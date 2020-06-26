/* CptS451, Team JJJ  */
DROP TABLE IF EXISTS Business CASCADE;
DROP TABLE IF EXISTS UserTable CASCADE;
DROP TABLE IF EXISTS CheckIn CASCADE;
DROP TABLE IF EXISTS Category CASCADE;
DROP TABLE IF EXISTS Review CASCADE;
DROP TABLE IF EXISTS OpenTimes CASCADE;
DROP TABLE IF EXISTS UserFavorite CASCADE;
DROP TABLE IF EXISTS UserFriend CASCADE;

CREATE TABLE Business (
	businessID varchar PRIMARY KEY,
	businessName varchar,
	address varchar,
	avgScore real,
	city varchar,
	numCheckins int,
	reviewCount int,
	reviewRating real,
	businessState varchar,
	stars real,
	openStatus int, /* 0 = closed, 1 = open */
	zip int,
	latitude real,
	longitude real
);

CREATE TABLE UserTable (
	userID varchar PRIMARY KEY,
	name varchar,
	avgStars real,
	yelpingSince date,
	latitude real,
	longitude real,
	numFans int,
	votes int
);

CREATE TABLE CheckIn (
	checkInDay varchar,
	checkInTime time,
	checkInAmount int,
	checkInBusinessID varchar NOT NULL,
	PRIMARY KEY (checkInBusinessID, checkInDay, checkInTime),
	FOREIGN KEY (checkInBusinessID) REFERENCES Business(businessID)

);

CREATE TABLE Category (
	businessID varchar,
	name varchar,
	PRIMARY KEY(businessID, name),
	FOREIGN KEY (businessID) REFERENCES Business(businessID)
);

CREATE TABLE Review (
	reviewID varchar PRIMARY KEY,
	userID varchar NOT NULL,
	businessID varchar NOT NULL,
	stars real,
	content char(2000)
);

CREATE TABLE OpenTimes (
	businessID varchar,
	day varchar,
	openTime time,
	closeTime time,
	PRIMARY KEY(businessID, day),
	FOREIGN KEY (businessID) REFERENCES Business(businessID)
);

CREATE TABLE UserFavorite (
	userID varchar,
	businessID varchar,
	PRIMARY KEY(userID, businessID),
	FOREIGN KEY (userID) REFERENCES UserTable(userID),
	FOREIGN KEY (businessID) REFERENCES Business(businessID)
);

CREATE TABLE UserFriend (
	userID varchar,
	friendUserID varchar,
	PRIMARY KEY (userID, friendUserID),
	FOREIGN KEY (userID) REFERENCES UserTable(userID),
	FOREIGN KEY (friendUserID) REFERENCES UserTable(userID)
);