module StockManagement.product

open System
open System.IO
open Newtonsoft.Json

type Product = {
    productId: int
    productName: string
    productPrice: decimal
    quantityInStock: int
    thresholdQuantity: int
}

let filePath = "products.json"

(*.....................................................................*)
let loadProducts filePath =
    if File.Exists(filePath) then
        let json = File.ReadAllText(filePath)
        JsonConvert.DeserializeObject<Product list>(json)
    else
        []

(*..............................................................................*)

// Function to save products back to JSON
let saveProducts filePath products =
    let json = JsonConvert.SerializeObject(products, Formatting.Indented)
    File.WriteAllText(filePath, json)


(*...................................................................................*)


let addProduct filePath newProduct =
    let products = loadProducts filePath
    let mutable idExists = false
    let mutable nameExists = false

    // Check if the product ID already exists
    for product in products do
        if product.productId = newProduct.productId then
            idExists <- true

    // Check if the product name already exists
    for product in products do
        if product.productName = newProduct.productName then
            nameExists <- true

    // Handle cases based on the checks
    if idExists then
        printfn "Error: Product ID %d already exists." newProduct.productId
    elif nameExists then
        printfn "Error: Product with name '%s' already exists." newProduct.productName
    else
        // Add the new product and save
        let updatedProducts = products @ [newProduct]//newProduct :: products
        saveProducts filePath updatedProducts
        printfn "Product added successfully!"



(*................................................................................*)

let updateProduct filePath updatedProduct =
    let productsList = loadProducts filePath // Load as list
    let products = productsList |> List.toArray // Convert the list into a mutable array
    let mutable productFound = false

    // Iterate over array indices
    for i in 0 .. products.Length - 1 do
        if products.[i].productId = updatedProduct.productId then
            products.[i] <- updatedProduct // Perform mutation here
            productFound <- true
            printfn "Product with ID %d has been updated." updatedProduct.productId

    if not productFound then
        printfn "Error: Product with ID %d not found." updatedProduct.productId
    else
        saveProducts filePath (products |> Array.toList) // Convert array back to list for saving


(*..........................................................................................*)


let getProducts filePath =
    if File.Exists(filePath) then
        let jsonContent = File.ReadAllText(filePath) // Read the JSON file
        JsonConvert.DeserializeObject<Product list>(jsonContent) // Deserialize JSON into list
    else
        [] // Return an empty list if no file is fou


(*...................................................................................................*)

// Imperative paradigm function to delete a product by productId
let deleteProduct filePath productIdToDelete =
    let productsList = loadProducts filePath // Load the products as a mutable array
    let products = productsList |> List.toArray // Convert the list into a mutable array
    let mutable productFound = false
    let mutable i = 0

    while i < products.Length && not productFound do
        if products.[i].productId = productIdToDelete then
            // Found the product, shift elements left
            for j in i .. products.Length - 2 do
                products.[j] <- products.[j + 1]
            
            // Shrink the array by 1 element
            let newProducts = Array.sub products 0 (products.Length - 1)
            saveProducts filePath newProducts // Save the new array to the JSON file
            printfn "Product with ID %d has been deleted." productIdToDelete
            productFound <- true
        else
            i <- i + 1

    if not productFound then
        printfn "Error: Product with ID %d not found." productIdToDelete


(*..........................................*)