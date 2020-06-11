/* CptS451, Team JJJ */


CREATE TABLE Business (
	businessID varchar PRIMARY KEY,
	avgScore real,
	city varchar,
	detailedInfo char(120),
	name varchar,
	numCheckins int,
	numReviews int,
	state varchar,
	stars real,
	zipcode int(5),

	/* TODO (Team): Add OpenTimes (should this be another entity?) */
	/* TODO (Team): Add category (I thought this was removed?) */
);

CREATE TABLE UserTable (
	userID varchar PRIMARY KEY,
	firstName varchar,
	lastName varchar,
	avgStars real,
	dateJoined Date,
	latitude real,
	longitude real,
	info char(120),
	isFanOf varchar, /* TODO (Team): Is this correct? */
	isFriendsWith varchar, /* TODO (Team): Is this correct? */
	numFans int,
	votes int,
	favorites varchar /* Does this need to point to a list of businessIDs? */
);

CREATE TABLE CheckIn (
	checkInDate date,
	checkInTime time,
	checkInBusinessID varchar NOT NULL,
	checkInUserId varchar NOT NULL
	PRIMARY KEY (checkInUserID, checkInBusinessID, checkInDate, checkInTime),
	FOREIGN KEY (checkInBusinessID) REFERENCES Business(businessID),
	FOREIGN KEY (checkInUserID) REFERENCES UserTable(userID)

	/* TODO (Team): Double check relationships are correct. */
);

CREATE TABLE Category (
	title varchar PRIMARY KEY
	/* TODO (Team): Handle categories relationship */
);

CREATE TABLE Review (
	reviewID varchar PRIMARY KEY,
	userID varchar NOT NULL,
	businessID varchar NOT NULL,
	stars real,
	text char(120)
);

