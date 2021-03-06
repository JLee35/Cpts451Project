CREATE OR REPLACE FUNCTION UpdateReviewCount() RETURNS trigger AS '
BEGIN
	UPDATE business
	SET reviewcount = reviewcount + 1
	WHERE business.businessid = new.businessid;
	RETURN new;
END
' LANGUAGE plpgsql;

CREATE TRIGGER ReviewCountInc
AFTER INSERT ON review
FOR EACH ROW
WHEN (new.businessid IS NOT NULL)
EXECUTE PROCEDURE UpdateReviewCount();

CREATE OR REPLACE FUNCTION UpdateReviewRating() RETURNS trigger AS '
BEGIN
	UPDATE business
	SET  reviewrating = ((SELECT SUM(review.stars) FROM review Where businessid = NEW.businessid) / business.reviewcount);
	RETURN NEW;
END
' LANGUAGE plpgsql;

CREATE TRIGGER ReviewRatingInc
AFTER INSERT OR UPDATE ON review 
FOR EACH ROW
WHEN (new.businessid IS NOT NULL)
EXECUTE PROCEDURE UpdateReviewRating();

CREATE OR REPLACE FUNCTION Updatecheckins() RETURNS trigger AS '
BEGIN 
	UPDATE business
	SET  numcheckins = (SELECT SUM(checkinamount) FROM Checkin WHERE checkinbusinessid =  NEW.checkinbusinessid )
   RETURN NEW;
END
' LANGUAGE plpgsql;

CREATE TRIGGER Checkins
BEFORE INSERT OR UPDATE ON checkin
FOR EACH ROW
WHEN (new.checkinbusinessid IS NOT NULL)
EXECUTE PROCEDURE Updatecheckins();

--Test statements

--Increases review count and increases review rating
INSERT INTO review(reviewid, userid, businessid, stars, content)
VALUES ('Ivjq9nkAt3_zuOwF94JwCD', 'ZkWIhSrKC2NA8aj0s4_8ew', 'Jy40ercZIQaNcz2qV3qgow', 5, 'Great Food!');

--Update previously entered review to reduce review rating.
UPDATE review
SET stars = 3
WHERE businessID = 'Jy40ercZIQaNcz2qV3qgow' AND  userID = 'ZkWIhSrKC2NA8aj0s4_8ew' AND reviewID = 'Ivjq9nkAt3_zuOwF94JwCD';

--Insert into checkin to increase checkin count
INSERT INTO checkin(checkinday, checkintime, checkinamount, checkinbusinessid)
VALUES ('Thursday', '22:30:00', 3, '-2X9U7v-Avoib-ki0y85bA');

UPDATE CheckIn
SET checkInAmount = 2
WHERE checkInBusinessID = '-2X9U7v-Avoib-ki0y85bA' AND checkInDay = 'Thursday' AND checkInTime = '22:30:00';

SELECT * FROM CheckIn WHERE checkInBusinessID = '-2X9U7v-Avoib-ki0y85bA';
SELECT numcheckins FROM Business WHERE BusinessID = '-2X9U7v-Avoib-ki0y85bA';

#code taken from https://stackoverflow.com/questions/24370975/find-distance-between-two-points-using-latitude-and-longitude-in-mysql
CREATE OR REPLACE FUNCTION myDistance( 
                            alat double precision,
                            alng double precision,
                            blat double precision,
                            blng double precision)
  RETURNS double precision
AS $$
      (select (6371 * acos( 
                cos( radians(blat) ) 
              * cos( radians( alat ) ) 
              * cos( radians( alng ) - radians(blng) ) 
              + sin( radians(blat) ) 
              * sin( radians( alat ) )
                ) ) as distance); 
$$  
LANGUAGE SQL;