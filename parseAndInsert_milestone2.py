import json
import sys
import psycopg2

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
    return psycopg2.connect("dbname='yelpdb' user='postgres' host='localhost' password='mustafa'")


def insert2BusinessTable():
    #reading the JSON file
    with open('./yelp_business.JSON','r') as f:    #TODO: update path for the input file
        line = f.readline()
        count_line = 0

        try:
            conn = getDBConnection()
        except Exception as error:
            printDBConnectionError()
            
        cur = conn.cursor()

        while line:
            data = json.loads(line)
            businessID = cleanStr4SQL(data['businessID'])

            sql_str = "INSERT INTO Business (businessID, businessName, address, avgScore, city, detailedInfo, numCheckins, reviewCount, reviewRating, businessState, stars, openStatus, zip) " \
                      "VALUES ('" + businessID + "','" + cleanStr4SQL(data['businessName']) + "','" + cleanStr4SQL(data['address']) + "','" \
                      str(data['avgScore']) + "','" + cleanStr4SQL(data['city']) + "','" + cleanStr4SQL(data['detailedInfo']) + "','" + "0"  \
                      + "','" + "0" + "','" + "0.0" + "','" + cleanStr4SQL(data['businessState']) + "','" + str(data['stars']) + "','" + str(data['openStatus']) |
                      + "','" + str(data['zip']) + ");"

            # Fill Categories table for each business.
            for item in data['categories']:
                insert2CategoryTable(businessID, cleanStr4SQL(item), conn, cur)

            # Fill OpenTimes table for each business.
            for key,value in data['hours'].items():
                openTime = value.split('-')[0]
                closeTime = value.split('-')[1]
                insert2OpenTimesTable(businessID, cleanStr4SQL(key), cleanStr4SQL(openTime), cleanStr4SQL(closeTime), conn, cur)
            
            try:
                cur.execute(sql_str)
            except Exception as error:
                printInsertError("Business", error)

            conn.commit()

            line = f.readline()
            count_line +=1

        cur.close()
        conn.close()

    print(count_line)
    f.close()


def insert2UserTable():
    with open('./yelp_user.JSON','r') as f:    #TODO: update path for the input file
        line = f.readline()
        count_line = 0

        try:
            conn = getDBConnection()
        except Exception as error:
            printDBConnectionError()

        cur = conn.cursor()

        while line:
            data = json.loads(line)

            userID = cleanStr4SQL(data['userID'])

            sql_str = "INSERT INTO UserTable (userID, firstName, lastName, avgStars, dateJoined, latitude, longitude, info, isFanOf, numFans, votes) " \
                      "VALUES ('" + userID + "','" + cleanStr4SQL(data['firstName']) + "','" + cleanStr4SQL(data['lastName']) + "','" + \
                      str(data['avgStars']) + "','" +  cleanStr4SQL(data['dateJoined']) + "','" + str(data['latitude']) + "','" + str(data['longitude']) + "','" +  \
                      cleanStr4SQL(data['info']) + "','" + cleanStr4SQL(data['isFanOf']) + "','" + \
                      str(data['numFans']) + "','" + str(data['votes']) + "','" + ");"

            # Fill UserFriend table with userID and each friendUserID.
            for item in data['friends']:
                insert2UserFriendTable(userID, cleanStr4SQL(item))

            # Fill UserFavorite table with userID and each favored businessID.
            for item in data['favorites']:
                insert2UserFavoriteTable(userID, cleanStr4SQL(item), conn, cur)

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
    with open('./yelp_checkin.JSON','r') as f:    #TODO: update path for the input file
        line = f.readline()
        count_line = 0

        try:
            conn = getDBConnection()
        except Exception as error:
            printDBConnectionError()

        cur = conn.cursor()

        while line:
            data = json.loads(line)

            sql_str = "INSERT INTO CheckIn (checkInDate, checkInTime, checkInBusinessID, checkInUserID) " \
                      "VALUES ('" + cleanStr4SQL(data['checkInDate']) + "','" + cleanStr4SQL(data['checkInTime']) + "','" +  \
                      cleanStr4SQL(data['checkInBusinessID']) + "','" + cleanStr4SQL(data['checkInUserID']) + ");"

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
    with open('./yelp_review.JSON','r') as f:    #TODO: update path for the input file
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
                      "VALUES ('" + cleanStr4SQL(data['reviewID']) + "','" + cleanStr4SQL(data['userID']) + "','" + \
                      cleanStr4SQL(data['businessID']) + "','" + str(data['stars']) + "','" + str(data['content']) + ");"

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
    "VALUES ('" + businessID + "','" + name + ");"

    try:
        connectionCursor.execute(sql_str)
    except Exception as error:
        printInsertError("Table", error)

    dbConnection.commit()


# Called by 'insert2BusinessTable' and populates OpenTimes table for each business.
def insert2OpenTimesTable(businessID, day, openTime, closeTime, dbConnection, connectionCursor):
    sql_str = "INSERT INTO OpenTimes (businessID, day, openTime, closeTime) " \
    "VALUES ('" + businessID + "','" + day + "','" + openTime + closeTime + ");"

    try:
        connectionCursor.execute(sql_str)
    except Exception as error:
        printInsertError("OpenTimes", error)

    dbConnection.commit()


# Called by 'insert2UserTable' and populates UserFavorites table for each user.
def insert2UserFavoriteTable(userID, businessID, dbConnection, connectionCursor):
    sql_str = "INSERT INTO UserFavorite (userID, businessID) " \
    "VALUES ('" + userID + businessID + ");"

    try:
        connectionCursor.execute(sql_str)
    except Exception as error:
        printInsertError("UserFavorite", error)

    dbConnection.commit()


# Called by 'insert2UserTable' and populates UserFriend table for each user.
def insert2UserFriendTable(userID, friendUserID):
    sql_str = "INSERT INTO UserFriend (userID, friendUserID) " \
    "VALUES ('" + userID + friendUserID + ");"

    try:
        connectionCursor.execute(sql_str)
    except Exception as error:
        printInsertError("UserFriend", error)

    dbConnection.commit()


insert2BusinessTable()
insert2UserTable()
insert2CheckInTable()
insert2ReviewTable()
insert2CategoryTable()
insert2OpenTimesTable()
insert2UserFavoriteTable()
insert2UserFriendTable()