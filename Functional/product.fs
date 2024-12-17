
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

let filePath = "Products.json"


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


let rec addProduct filePath newProduct =
    let products = loadProducts filePath

    let rec idExists productList =
        match productList with
        | [] -> false
        | head :: tail ->
            if head.productId = newProduct.productId then true
            else idExists tail

    let rec nameExists productList =
        match productList with
        | [] -> false
        | head :: tail ->
            if head.productName = newProduct.productName then true
            else nameExists tail

    if idExists products then
        printfn "Error: Product ID %d already exists." newProduct.productId
    elif nameExists products then
        printfn "Error: Product with name '%s' already exists." newProduct.productName
    else
        let rec addToEnd productList newProduct =
            match productList with
            | [] -> [newProduct]
            | head :: tail -> head :: addToEnd tail newProduct

        let updatedProducts = addToEnd products newProduct
        saveProducts filePath updatedProducts
        printfn "Product added successfully!"

(*................................................................................*)

let rec updateProduct filePath updatedProduct =
    let products = loadProducts filePath

    let rec update productsList =
        match productsList with
        | [] -> ([], false)
        | head :: tail ->
            let (updatedTail, found) = update tail
            if head.productId = updatedProduct.productId then
                (updatedProduct :: updatedTail, true)
            else
                (head :: updatedTail, found)

    let (updatedProducts, productFound) = update products

    if not productFound then
        printfn "Error: Product with ID %d not found." updatedProduct.productId
    else
        saveProducts filePath updatedProducts
        printfn "Product with ID %d has been updated." updatedProduct.productId

(*..........................................................................................*)

let rec getProducts filePath =
    loadProducts filePath

(*...................................................................................................*)


let rec deleteProduct filePath productIdToDelete =
    let products = loadProducts filePath

    let rec delete productsList =
        match productsList with
        | [] -> ([], false)
        | head :: tail ->
            let (remainingProducts, found) = delete tail
            if head.productId = productIdToDelete then
                (remainingProducts, true)
            else
                (head :: remainingProducts, found)

    let (updatedProducts, productFound) = delete products

    if not productFound then
        printfn "Error: Product with ID %d not found." productIdToDelete
    else
        saveProducts filePath updatedProducts
        printfn "Product with ID %d has been deleted." productIdToDelete

(*..........................................*)