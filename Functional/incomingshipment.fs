module StockManagement.incomingshipment


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

(*...................................*)

let loadShipments filePath =
    if File.Exists filePath then
        let json = File.ReadAllText filePath
        JsonConvert.DeserializeObject<Shipment[]>(json)
    else
        [||]

(*........................................*)



let loadProductstoArr filePath =
    if File.Exists filePath then
        let json = File.ReadAllText filePath
        JsonConvert.DeserializeObject<Product[]>(json) // Deserialize JSON to a Product array
    else
        [||] // Return an empty array if the file doesn't exist

(*.......................................................*)



let saveShipments filePath (shipments: Shipment[]) =
    let json = JsonConvert.SerializeObject(shipments, Formatting.Indented)
    File.WriteAllText(filePath, json)


(*.....................................................*)

// Function to check if a shipment with the same ID already exists
let rec shipmentExists shipmentId (shipments: Shipment[]) =
    match shipments with
    | [||] -> false
    | _ when shipments.[0].shipmentId = shipmentId -> true
    | _ -> shipmentExists shipmentId (Array.tail shipments)



(*..........................................................*)

// Function to find a product by ID
let rec findProductById productId (products: Product[]) =
    match products with
    | [||] -> None
    | _ when products.[0].productId = productId -> Some products.[0]
    | _ -> findProductById productId (Array.tail products)


(*................................................................*)

// Function to update a product in the array
let rec updateProductArray (updatedProduct:Product) (products: Product[]) =
    match products with
    | [||] -> [||]
    | _ when products.[0].productId = updatedProduct.productId ->
        Array.append [| updatedProduct |] (Array.tail products)
    | _ -> Array.append [| products.[0] |] (updateProductArray updatedProduct (Array.tail products))


(*..............................................................*)

// Function to add a new shipment
let rec addShipment shipments newShipment =
    match shipments with
    | [||] -> [| newShipment |]
    | _ -> Array.append [| shipments.[0] |] (addShipment (Array.tail shipments) newShipment)


(*...............................................*)

// Main function to add an incoming shipment
let addIncomingShipment filePath newShipment =
    let shipments = loadShipments filePath
    let products = loadProductstoArr "Products.json"

    if shipmentExists newShipment.shipmentId shipments then
        printfn "Error: Shipment with ID %d already exists." newShipment.shipmentId
    else
        match findProductById newShipment.productId products with
        | None ->
            printfn "Error: Product with ID %d not found." newShipment.productId
        | Some product ->
            // Update the product stock
            let updatedProduct = {
                productId = product.productId
                productName = product.productName
                productPrice = product.productPrice
                quantityInStock = product.quantityInStock + newShipment.quantityReceived
                thresholdQuantity = product.thresholdQuantity
            }

            // Save updated shipments
            let updatedShipments = addShipment shipments newShipment
            saveShipments filePath updatedShipments

            // Save updated product
            let updatedProducts = updateProductArray updatedProduct products
            saveProducts "products.json" updatedProducts

            printfn "Shipment with ID %d added successfully." newShipment.shipmentId
