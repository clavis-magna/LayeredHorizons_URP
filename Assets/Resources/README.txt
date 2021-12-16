//Formatting Data//

- There are 2 essential data points that are required for displaying data.
“latitude”
“longitude”	

- The latitude and longitude column must contain a float value e.g. instead of -7 put -7.0.

- Check the minus value in the latitude and longitude. (some are misplaced)

- If the dataset has a null value put 0.0 in as a placeholder.

- Make sure that the latitude and longitude do not spread out further than 3-5 degrees each way. For 
each dataset they can only accommodate a few sections so make a new csv file if you need to cover more distance.


//Adding Data//

- Place CSV files in the “Assets/StreamingAssets” folder.

- Added a JSON file in “Assets/Resources” file path titled “Edit_DataFileNames.json"

- For each CSV file to add to the project, add an item to the JSON file.

- Each item needs two values “fileName” and “headerColumnName”

- Add the CSV file name to “fileName” INCLUDING the “.csv” suffix.

- Add the header file of the name labels that you would like to display in the “headerColumnName”.

- Check the JSON to make sure that the commas are in the right place. They should be after every element in a list except the last one.

- To make things easier just copy and paste a new file and check the commas are correct.
