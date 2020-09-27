# PharmacyLocator
This app is designed to get nearest pharmacy from the user location provide in latitude and longitude

Main Service LocatorService.cs, Location for the file is https://github.com/harshulr/PharmacyLocator/tree/master/PharmacyLocator/Services

Running the project
  Clone the project
  Load the project by clicking on PharmacyLocator.sln
  Build and Run the project
  For getting the result, we can use url /home/locate?latitude={userlatitude}&longitude={userlongitude} ,
                          example:       /home/locate?latitude=39.05&longitude=-95.73

  Concept: 
    Once the system gets user co-ordinates, system adds and substracts 0.5 degrees to both latitude and longitude
    to make a radius around the user (We can make it 1 degree if we want larger radius or change according to our needs, but since difference between
    the max and min co-ordinates of pharmacies were less, system uses only 0.5). 
    
    If there are pharmacies within our radius, then check the distance between the user and pharmacy.
    If there are no pharmacies then check user distance against all the pharmacy.(Another thing I thought was, We can increase the radius and 
    keep increasing till we find a pharmacy, but since the co-ordinates were so near and not many pharmacies I went with checking distance of user to all the pharmacies)
    
    Return the nearest pharmacy as JSON object.

    Two screenshots of the test cases are attached on the main page of the project with name Example.png, Example2.png
    
