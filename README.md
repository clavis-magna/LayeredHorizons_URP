# LayeredHorizons_URP

//DATA PREREQUISITES//

- There are 2 essential data points that are required for displaying data.
“latitude”
“longitude”	

- The latitude and longitude column must contain a float value e.g. instead of -7 put -7.0.

- Check the minus value in the latitude and longitude and use the values from -180 to 180

- If the dataset has a null value leave the box empty.

- Each CSV file is controlled together in the project. i.e. you can switch a dataset on/off, change the colours etc.


//INSTRUCTIONS FOR ADDING DATA//

1. Place CSV files in the “Assets/StreamingAssets” folder.

2. In the "Assets" folder open the JSON file titled “Edit_DataFileNames.json".

3. To add a dataset to the project, create a new item and apply the four values into each item.
        
        "fileName": "bambooTESTONLY.csv",         
        (Add file name here INCLUDE .csv on the end)
        
        "headerColumnName": "language",           
        (The string in this column will be used as the text labels to represent the data)
        
        "deformationScaleColumnName": "Number",   
        (optional: Header Name of a column with a number from 0-1, this is used to determine by how much does the entry make a deformation. If no value is specified each entry will default to 0.5)
        
        "additiveMesh": "false"                   
        (Boolean to determine if the deformations are additive and will result in each entry being added on top of each other)
        
        //TYPES OF DATASETS//
        Use this as a guide to determine which settings suit your dataset the best.
        
        "deformationScaleColumnName": "Number", + "additiveMesh": "false"
        ![alt text](https://github.com/jakemu6/LayeredHorizons_URP/blob/main/ImpFalse.png?raw=true)
        
        "deformationScaleColumnName": "Number", + "additiveMesh": "true"
        ![alt text](https://github.com/jakemu6/LayeredHorizons_URP/blob/main/ImpTrue.png?raw=true)
        
        "deformationScaleColumnName": "", + "additiveMesh": "false"
        ![alt text](https://github.com/jakemu6/LayeredHorizons_URP/blob/main/NONFalse.png?raw=true)
        
        "deformationScaleColumnName": "", + "additiveMesh": "true"
        ![alt text](https://github.com/jakemu6/LayeredHorizons_URP/blob/main/NONTrue.png?raw=true)
        
4. Check the JSON to make sure that the commas are in the right place. They should be after every element except the last one.
