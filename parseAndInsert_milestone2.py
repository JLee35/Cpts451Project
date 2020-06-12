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

def insert2BusinessTable():
    #reading the JSON file
    with open('./yelp_business.JSON','r') as f:    #TODO: update path for the input file
        line = f.readline()
        count_line = 0

        try:
            conn = psycopg2.connect("dbname='yelpdb' user='postgres' host='localhost' password='mustafa'")
        except Exception as error:
            print('ERROR: Unable to connect to the database!')
            print('Error message:', error)
        cur = conn.cursor()

        while line:
            data = json.loads(line)

            # TODO: Associate each businessID with OpenTimes table.
            # See 'milestone2Schema.sql' for details.

            sql_str = "INSERT INTO Business (businessID, businessName, address, avgScore, city, detailedInfo, numCheckins, numReviews, businessState, stars, openStatus, zip) " \
                      "VALUES ('" + clearnStr4SQL(data['businessID']) + "','" + clearnStr4SQL(data['businessName']) + "','" + clearnStr4SQL(data['address']) + "','" \
                      str(data['avgScore']) + "','" + cleanStr4SQL(data['city']) + "','" + cleanStr4SQL(data['detailedInfo']) + "','" + str(data['numCheckins'])  \
                      + "','" + str(data['numReviews']) + "','" + cleanStr4SQL(data['businessState']) + "','" + str(data['stars']) + "','" + str(data['openStatus']) |
                      + "','" + str(data['zip']) + ");"

            try:
                cur.execute(sql_str)
            except Exception as error:
                print('ERROR: Insert to Business table failed!')
                print('Error message:', error)

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
            conn = psycopg2.connect("dbname='yelpdb' user='postgres' host='localhost' password='mustafa'")
        except Exception as error:
            print('ERROR: Unable to connect to the database!')
            print('Error message:', error)
        cur = conn.cursor()

        while line:
            data = json.loads(line)

            # TODO: If you look at the schema file, UserFriend is a table that stores a userID and friendUserID.
            # The data also contains user favorites, but that also needs to be stored in a seperate table.
            # We need to populate n number of tables for each of the user's friends and favorites.
            # See 'milestone2Schema.sql' for details.

            sql_str = "INSERT INTO UserTable (userID, firstName, lastName, avgStars, dateJoined, latitude, longitude, info, isFanOf, numFans, votes) " \
                      "VALUES ('" + clearnStr4SQL(data['userID']) + "','" + clearnStr4SQL(data['firstName']) + "','" + clearnStr4SQL(data['lastName']) + "','" + \
                      str(data['avgStars']) + "','" +  clearnStr4SQL(data['dateJoined']) + "','" + str(data['latitude']) + "','" + str(data['longitude']) + "','" +  \
                      clearnStr4SQL(data['info']) + "','" + clearnStr4SQL(data['isFanOf']) + "','" + \
                      str(data['numFans']) + "','" + str(data['votes']) + "','" + ");"

            try:
                cur.execute(sql_str)
            except Exception as error:
                print('ERROR: Insert to UserTable failed!')
                print('Error message:', error)

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
            conn = psycopg2.connect("dbname='yelpdb' user='postgres' host='localhost' password='mustafa'")
        except Exception as error:
            print('ERROR: Unable to connect to the database!')
            print('Error message:', error)
        cur = conn.cursor()

        while line:
            data = json.loads(line)

            sql_str = "INSERT INTO CheckIn (checkInDate, checkInTime, checkInBusinessID, checkInUserID) " \
                      "VALUES ('" + clearnStr4SQL(data['checkInDate']) + "','" + clearnStr4SQL(data['checkInTime']) + "','" +  \
                      clearnStr4SQL(data['checkInBusinessID']) + "','" + clearnStr4SQL(data['checkInUserID']) + ");"

            try:
                cur.execute(sql_str)
            except Exception as error:
                print('ERROR: Insert to CheckIn table failed!')
                print('Error message:', error)

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
            conn = psycopg2.connect("dbname='yelpdb' user='postgres' host='localhost' password='mustafa'")
        except Exception as error:
            print('ERROR: Unable to connect to the database!')
            print('Error message:', error)
        cur = conn.cursor()

        while line:
            data = json.loads(line)
            
            sql_str = "INSERT INTO Review (reviewID, userID, businessID, stars, content) " \
                      "VALUES ('" + clearnStr4SQL(data['reviewID']) + "','" + clearnStr4SQL(data['userID']) + "','" + \
                      clearnStr4SQL(data['businessID']) + "','" + str(data['stars']) + "','" + str(data['content']) + ");"

            try:
                cur.execute(sql_str)
            except Exception as error:
                print('ERROR: Insert to Review table failed!')
                print('Error message:', error)

            conn.commit()

            line = f.readline()
            count_line +=1

        cur.close()
        conn.close()

    print(count_line)
    f.close()

def insert2CategoryTable():
    # Check 'milestone2Schema.sql' for what needs to be stored here.


def insert2OpenTimesTable():
    # Check 'milestone2Schema.sql' for what needs to be stored here.


def insert2UserFavoriteTable():
    # Check 'milestone2Schema.sql' for what needs to be stored here.


def insert2UserFriendTable():
    # Check 'milestone2Schema.sql' for what needs to be stored here.    


insert2BusinessTable()
insert2UserTable()
insert2CheckInTable()
insert2ReviewTable()
insert2CategoryTable()
insert2OpenTimesTable()
insert2UserFavoriteTable()
insert2UserFriendTable()