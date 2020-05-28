    CREATE TABLE yelpUser (
        yelpUserID INTEGER PRIMARY KEY,
        latitude REAL,
        longitude REAL,
        firstName VARCHAR,
        lastName VARCHAR,
        votes INTEGER,
        avgStars REAL,
        numFans INTEGER,
        dateJoined Date,
        info VARCHAR
    );

    CREATE TABLE friends (
        yelpUserID INTEGER, 
        friendID INTEGER,
        PRIMARY KEY (yelpUserID, friendID),
        FOREIGN KEY (yelpUserID) REFERENCES yelpUser(yelpUserID)
    );

    CREATE TABLE fans (
        yelpUserID INTEGER, 
        fanID INTEGER,
        PRIMARY KEY (yelpUserID, fanID),
        FOREIGN KEY (yelpUserID) REFERENCES yelpUser(yelpUserID)
    );

    CREATE TABLE tip (
        yelpUserID INTEGER, 
        tipID INTEGER,
        tipContent VARCHAR,
        PRIMARY KEY (yelpUserID, tipID),
        FOREIGN KEY (yelpUserID) REFERENCES yelpUser(yelpUserID)
    );

    CREATE TABLE business (
        businessID INTEGER PRIMARY KEY,
        numCheckins INTEGER,
        businessName VARCHAR,
        city VARCHAR, 
        state VARCHAR,
        zipcode INTEGER,
        info VARCHAR,
        popularity REAL,
        rating REAL,
        numReviews INTEGER
    );

    CREATE TABLE favorites (
        yelpUserID INTEGER,
        businessID INTEGER,
        PRIMARY KEY (yelpUserID, businessID),
        FOREIGN KEY (yelpUserID) REFERENCES yelpUser(yelpUserID),
        FOREIGN KEY (businessID) REFERENCES business(businessID)
    );

    CREATE TABLE businessHours (
        businessID INTEGER,
        businessHours VARCHAR,
        businessDay VARCHAR,
        PRIMARY KEY (businessID, businessHours, businessDay),
        FOREIGN KEY (businessID) REFERENCES business(businessID)
    );

    CREATE TABLE categories (
        businessID INTEGER,
        title VARCHAR, 
        PRIMARY KEY (businessID, title),
        FOREIGN KEY (businessID) REFERENCES business(businessID)
    );

    CREATE TABLE reviews (
        yelpUserID INTEGER,
        businessID INTEGER,
        reviewID INTEGER,
        reviewContent VARCHAR,
        stars REAL,
        PRIMARY KEY (yelpUserID, businessID, reviewID),
        FOREIGN KEY (yelpUserID) REFERENCES yelpUser(yelpUserID),
        FOREIGN KEY (businessID) REFERENCES business(businessID)
    );