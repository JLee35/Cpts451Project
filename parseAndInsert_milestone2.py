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
        #outfile =  open('./yelp_business.SQL', 'w')  #uncomment this line if you are writing the INSERT statements to an output file.
        line = f.readline()
        count_line = 0

        #connect to yelpdb database on postgres server using psycopg2
        #TODO: update the database name, username, and password
        try:
            conn = psycopg2.connect("dbname='yelpdb' user='postgres' host='localhost' password='none'")
        except Exception as error:
            print('ERROR: Unable to connect to the database!')
            print('Error message:', error)
        cur = conn.cursor()

        while line:
            data = json.loads(line)
            # Generate the INSERT statement for the current business
            # TODO: The below INSERT statement is based on a simple (and incomplete) businesstable schema. Update the statement based on your own table schema and
            # include values for all businessTable attributes
            sql_str = "INSERT INTO Business (businessID, businessName, address, avgScore, city, detailedInfo, numCheckins, numReviews, businessState, stars, openStatus, zip) " \
                      "VALUES ('" + clearnStr4SQL(data['businessID']) + "','" + clearnStr4SQL(data['businessName']) + "','" + clearnStr4SQL(data['address']) + "','" \
                      str(data['avgScore']) + "','" + cleanStr4SQL(data['city']) + "','" + cleanStr4SQL(data['detailedInfo']) + "','" + str(data['numCheckins'])  \
                      + "','" + str(data['numReviews']) + "','" + cleanStr4SQL(data['businessState']) + "','" + str(data['stars']) + "','" + str(data['openStatus']) |
                      + "','" + str(data['zip']) + ");"
                      
            try:
                cur.execute(sql_str)
            except Exception as error:
                print('ERROR: Insert to business table failed!')
                print('Error message:', error)

            conn.commit()
            # optionally you might write the INSERT statement to a file.
            # outfile.write(sql_str)

            line = f.readline()
            count_line +=1

        cur.close()
        conn.close()

    print(count_line)
    #outfile.close()  #uncomment this line if you are writing the INSERT statements to an output file.
    f.close()


insert2BusinessTable()