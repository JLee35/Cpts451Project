import json
import sys
import psycopg2

# TODO: Update path for input files.
dataPath = '/Users/admin/Desktop/school/cpts451/yelpdata/'

def cleanStr4SQL(s):
	return s.replace("'","`").replace("\n"," ")

def int2BoolStr (value):
	if value == 0:
		return 'False'
	else:
		return 'True'

def printDBConnectionError(error):
	print('ERROR: Unable to connect to the database!')
	print('Error message:', error)


def printInsertError(tableName, error):
	print('ERROR: Insert to ' + tableName + ' table failed!')
	print('Error message:', error)


def getDBConnection():
	# TODO: Update dbname, user, host, and password for your machine.
	return psycopg2.connect("dbname='yelpdb' user='postgres' host='localhost' password='mustafa'")


def insert2BusinessTable():
	#reading the JSON file
	print('inserting business')
	with open(dataPath + 'yelp_business.JSON','r') as f:
		line = f.readline()
		count_line = 0

		try:
			conn = getDBConnection()
		except Exception as error:
			printDBConnectionError()
			
		cur = conn.cursor()

		while line:
			data = json.loads(line)
			businessID = cleanStr4SQL(data['business_id'])

			sql_str = "INSERT INTO Business (businessID, businessName, address, avgScore, city, numCheckins, reviewCount, reviewRating, businessState, stars, openStatus, zip) " \
                      + "VALUES ('" + businessID + "','" + cleanStr4SQL(data['name']) + "','" + cleanStr4SQL(data['address']) + "','" \
                      + cleanStr4SQL(data['avgScore']) + "','" + cleanStr4SQL(data['city']) + "','" + str(0)  \
                      + "','" + str(0) + "','" + str(0) + "','" + cleanStr4SQL(data['state']) + "','" + str(data['stars']) + "','" + str(data['is_open']) \
                      + "','" + str(data['postal_code']) + "');"

			try:
				cur.execute(sql_str)
			except Exception as error:
				printInsertError("Business", error)

			# Fill Categories table for each business.
			for item in data['categories']:
				insert2CategoryTable(businessID, cleanStr4SQL(item), conn, cur)

			# Fill OpenTimes table for each business.
			for key,value in data['hours'].items():
				openTime = value.split('-')[0]
				closeTime = value.split('-')[1]
				insert2OpenTimesTable(businessID, cleanStr4SQL(key), cleanStr4SQL(openTime), cleanStr4SQL(closeTime), conn, cur)

			conn.commit()

			line = f.readline()
			count_line +=1

		cur.close()
		conn.close()

	print(count_line)
	f.close()


def insert2UserTable():
	print('inserting user')
	with open(dataPath + 'yelp_user.JSON','r') as f:
		line = f.readline()
		count_line = 0

		try:
			conn = getDBConnection()
		except Exception as error:
			printDBConnectionError()

		cur = conn.cursor()

		while line:
			data = json.loads(line)

			userID = cleanStr4SQL(data['user_id'])

            sql_str = "INSERT INTO UserTable (userID, name, avgStars, yelpingSince, latitude, longitude, numFans, votes) " \
                      "VALUES ('" + userID + "','" + cleanStr4SQL(data['name']) + "','" + str(data['average_stars']) + "','" + \
                      cleanStr4SQL(data['yelping_since']) + "','" + str(0) + "','" + str(0) + "','" + str(data['fans']) + "','" + str(data['review_count']) + "');"

			try:
				cur.execute(sql_str)
			except Exception as error:
				printInsertError("UserTable", error)

			conn.commit()

			line = f.readline()
			count_line +=1

		cur.close()
		conn.close()

	print(count_line)
	f.close()


def insert2CheckInTable():
	print('inserting check-in')
	with open(dataPath + 'yelp_checkin.JSON','r') as f:
		line = f.readline()
		count_line = 0

		try:
			conn = getDBConnection()
		except Exception as error:
			printDBConnectionError()

		cur = conn.cursor()

		while line:
			data = json.loads(line)
			business_id = cleanStr4SQL(data['business_id'])
			for day in data['time']:
				for hour in data['time'][day]:
					sql_str = "INSERT INTO CheckIn (checkInDay, checkInTime, checkInAmount, checkInBusinessID) " + \
					  "VALUES ('" + cleanStr4SQL(day) + "','" + cleanStr4SQL(hour) + "','" + str(data['time'][day][hour]) + "','" + \
					  business_id + "');"
					try:
						cur.execute(sql_str)
					except Exception as error:
						printInsertError("CheckIn", error)

			conn.commit()

			line = f.readline()
			count_line +=1

		cur.close()
		conn.close()

	print(count_line)
	f.close()


def insert2ReviewTable():
	print('inserting review')
	with open(dataPath + 'yelp_review.JSON','r') as f:
		line = f.readline()
		count_line = 0

		try:
			conn = getDBConnection()
		except Exception as error:
			printDBConnectionError()

		cur = conn.cursor()

		while line:
			data = json.loads(line)
			
			sql_str = "INSERT INTO Review (reviewID, userID, businessID, stars, content) " \
					  "VALUES ('" + cleanStr4SQL(data['review_id']) + "','" + cleanStr4SQL(data['user_id']) + "','" + \
					  cleanStr4SQL(data['business_id']) + "','" + str(data['stars']) + "','" + cleanStr4SQL(data['text']) + "');"

			try:
				cur.execute(sql_str)
			except Exception as error:
				printInsertError("Review", error)

			conn.commit()

			line = f.readline()
			count_line +=1

		cur.close()
		conn.close()

	print(count_line)
	f.close()


# Called by 'insert2BusinessTable' and populates Category Table for each business.
def insert2CategoryTable(businessID, name, dbConnection, connectionCursor):
	sql_str = "INSERT INTO Category (businessID, name) " \
	"VALUES ('" + businessID + "','" + name + "');"

	try:
		connectionCursor.execute(sql_str)
	except Exception as error:
		printInsertError("Table", error)

	dbConnection.commit()


# Called by 'insert2BusinessTable' and populates OpenTimes table for each business.
def insert2OpenTimesTable(businessID, day, openTime, closeTime, dbConnection, connectionCursor):
	sql_str = "INSERT INTO OpenTimes (businessID, day, openTime, closeTime) " \
	"VALUES ('" + businessID + "','" + day + "','" + openTime + "','" + closeTime + "');"

	try:
		connectionCursor.execute(sql_str)
	except Exception as error:
		printInsertError("OpenTimes", error)

	dbConnection.commit()


# Called by 'insert2UserTable' and populates UserFavorites table for each user.
def insert2UserFavoriteTable(userID, businessID, dbConnection, connectionCursor):
	sql_str = "INSERT INTO UserFavorite (userID, businessID) " \
	"VALUES ('" + userID + businessID + "');"

	try:
		connectionCursor.execute(sql_str)
	except Exception as error:
		printInsertError("UserFavorite", error)

	dbConnection.commit()


# Called by 'insert2UserTable' and populates UserFriend table for each user.
def insert2UserFriendTable():
	print('inserting user friend')
	with open(dataPath + 'yelp_user.JSON','r') as f:
		line = f.readline()
		count_line = 0

		try:
			conn = getDBConnection()
		except Exception as error:
			printDBConnectionError()

		cur = conn.cursor()

		while line:
			data = json.loads(line)

			userID = cleanStr4SQL(data['user_id'])
			for item in data['friends']:
				sql_str = "INSERT INTO UserFriend(userID, friendUserID) " \
					  "VALUES ('" + userID + "','" + cleanStr4SQL(item) + "');"

				try:
					cur.execute(sql_str)
				except Exception as error:
					printInsertError("UserFriend", error)

				conn.commit()

			line = f.readline()
			count_line +=1

		cur.close()
		conn.close()

	print(count_line)
	f.close()


insert2BusinessTable()
insert2UserTable()
insert2CheckInTable()
insert2ReviewTable()
#insert2UserFavoriteTable()
insert2UserFriendTable()