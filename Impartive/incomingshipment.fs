module StockManagement.incomingshippment

open System
open System.IO
open Newtonsoft.Json
open StockManagement.product


type Shipment = {
    shipmentId: int
    shipmentDate: DateTime
    quantityReceived: int
    productId: int
}


let filePath="IncomingShipments.json"




(*................................................................*)

// Function to load existing shipments
let loadShipments filePath =
    if File.Exists(filePath) then
        let json = File.ReadAllText(filePath)
        JsonConvert.DeserializeObject<Shipment array>(json) // Deserialize JSON to array
    else
        [||] // Return empty array if no file exists



(*....................................................................*)




// Function to save updated shipments back to JSON
let saveShipments filePath (shipments: Shipment array) =
    let json = JsonConvert.SerializeObject(shipments, Formatting.Indented)
    File.WriteAllText(filePath, json)




(*.....................................................................*)



// Function to add a new incoming shipment using the imperative paradigm
let addIncomingShipment filePath newShipment =
    let shipments = loadShipments filePath // Load existing shipments
    let products=loadProducts "products.json"
    let mutable shipmentExists = false
    let mutable productExists = false

    // Check for uniqueness using imperative for-loop
    for i in 0 .. shipments.Length - 1 do
        if shipments.[i].shipmentId = newShipment.shipmentId then
            shipmentExists <- true

    if shipmentExists then
        printfn "Error: Shipment with ID %d already exists." newShipment.shipmentId
    else
        // Add the new shipment to the array (simulate appending at the end)
       
        
        for product in products do
          if not productExists && product.productId = newShipment.productId then
            let newProduct = {
                  productId =product.productId
                  productName = product.productName
                  productPrice = product.productPrice
                  quantityInStock =product.quantityInStock+newShipment.quantityReceived
                  thresholdQuantity = product.thresholdQuantity
                  }
            

            let newShipmentsArray = Array.append shipments [| newShipment |]
            saveShipments filePath newShipmentsArray

            
            
            updateProduct "products.json" newProduct
            productExists<-true
              




        if not productExists then
          printfn "Error: Product with ID %d not found." newShipment.productId
        
        else
            printfn "Shipment with ID %d added successfully." newShipment.shipmentId



(*............................................................................*)