import json

def cleanStr4SQL(s):
		s = str(s)
		return s.replace("'","`").replace("\n"," ")

def parseBusinessData():
	#read the JSON file
	with open('yelp_business.JSON','r') as f:  #Assumes that the data files are available in the current directory. If not, you should set the path for the yelp data files.
		outfile =  open('business.txt', 'w')
		line = f.readline()
		count_line = 0
		#read each JSON abject and extract data
		while line:
			data = json.loads(line)
			outfile.write(cleanStr4SQL(data['business_id'])+'\t') #business id
			outfile.write(cleanStr4SQL(data['name'])+'\t') #name
			outfile.write(cleanStr4SQL(data['address'])+'\t') #full_address
			outfile.write(cleanStr4SQL(data['state'])+'\t') #state
			outfile.write(cleanStr4SQL(data['city'])+'\t') #city
			outfile.write(cleanStr4SQL(data['postal_code']) + '\t')  #zipcode
			outfile.write(str(data['latitude'])+'\t') #latitude
			outfile.write(str(data['longitude'])+'\t') #longitude
			outfile.write(str(data['stars'])+'\t') #stars
			outfile.write(str(data['review_count'])+'\t') #reviewcount
			outfile.write(str(data['is_open'])+'\t') #openstatus
			outfile.write(cleanStr4SQL([item for item in data['categories']])+'\t') #category list
			
			for key, value in data['hours'].items():
				outfile.write(str(key + ':' + value)+'\t') #hours
			
			outfile.write('\n');

			line = f.readline()
			count_line +=1
	print(count_line)
	outfile.close()
	f.close()

def parseUserData():
	#write code to parse yelp_user.JSON
	with open('yelp_user.JSON', 'r') as f:
		outfile = open('user.txt', 'w')
		line = f.readline()
		count_line = 0
		#read each JSON object and extract data
		while line:
			data = json.loads(line)
			outfile.write(cleanStr4SQL(data['average_stars'])+'\t') #average stars
			outfile.write(cleanStr4SQL(data['cool'])+'\t') #cool
			outfile.write(cleanStr4SQL(data['fans'])+'\t') #fans
			outfile.write(str([item for item in data['friends']])+'\t') #friends list
			outfile.write(cleanStr4SQL(data['funny'])+'\t') #funny
			outfile.write(cleanStr4SQL(data['name'])+'\t') #name
			outfile.write(cleanStr4SQL(data['review_count'])+'\t') #review count
			outfile.write(cleanStr4SQL(data['useful'])+'\t') #useful
			outfile.write(cleanStr4SQL(data['user_id'])+'\t') #user id
			outfile.write(cleanStr4SQL(data['yelping_since'])+'\t') #yelping since

			line = f.readline()
			count_line +=1
		print(count_line)
		outfile.close()
		f.close()

def parseCheckinData():
	#write code to parse yelp_checkin.JSON
	with open('yelp_checkin.JSON', 'r') as f:
		outfile = open('checkin.txt', 'w')
		line = f.readline()
		count_line = 0
		#read each JSON object and extract data
		while line:
			data = json.loads(line)
			# write code for checkin times
			for day, time in data['time'].items():
				outfile.write(day + ': \t') 
				for time, checkins in time.items():
					outfile.write(str(time) + ':' + str(checkins) + ' ') #checkin times
				outfile.write('\n');
					
			outfile.write(cleanStr4SQL(data['business_id'])+'\t') #business id
			outfile.write('\n');

			line = f.readline()
			count_line += 1
		print(count_line)
		outfile.close()
		f.close()

def parseReviewData():
	#write code to parse yelp_review.JSON
	with open('yelp_review.JSON', 'r') as f:
		outfile = open('review.txt', 'w')
		line = f.readline()
		count_line = 0
		#read each JSON object and extract data
		while line:
			data = json.loads(line)
			outfile.write(cleanStr4SQL(data['review_id'])+'\t') #review id
			outfile.write(cleanStr4SQL(data['user_id'])+'\t') #user id
			outfile.write(cleanStr4SQL(data['business_id'])+'\t') #business id
			outfile.write(cleanStr4SQL(data['stars'])+'\t') #stars
			outfile.write(cleanStr4SQL(data['date'])+'\t') #date
			outfile.write(cleanStr4SQL(data['text'])+'\t') #text
			outfile.write(cleanStr4SQL(data['useful'])+'\t') #useful
			outfile.write(cleanStr4SQL(data['funny'])+'\t') #funny
			outfile.write(cleanStr4SQL(data['cool'])+'\t') #cool

			line = f.readline()
			count_line += 1
		print(count_line)
		outfile.close()
		f.close()

parseBusinessData()
parseUserData()
parseCheckinData()
parseReviewData()
