
module StockManagement.report

open StockManagement.product
open StockManagement.order

open System
open System.IO
open Newtonsoft.Json

let loadProductstoArr filePath =
    if File.Exists filePath then
        let json = File.ReadAllText filePath
        JsonConvert.DeserializeObject<Product[]>(json) // Deserialize JSON to a Product array
    else
        [||] // Return an empty array if the file doesn't exist



(*.............................................................................*)

// Helper function to find products with low stock
let rec findLowStockItems products =
    match products with
    | [||] -> []
    | _ when products.[0].quantityInStock < products.[0].thresholdQuantity ->
        (products.[0].productName, products.[0].quantityInStock) :: findLowStockItems (Array.tail products)
    | _ -> findLowStockItems (Array.tail products)


(*......................................................................................*)

// Function to check and display low stock items
let checkLowStockItems () =
    let products = loadProductstoArr "Products.json"
    let lowStockItems = findLowStockItems products
    match lowStockItems with
    | [] -> printfn "No low stock items found."
    | _ ->
        for (name, quantity) in lowStockItems do
            printfn "This product has low stock: Product: %s, Quantity: %d" name quantity

(*.............................................................................*)

// Recursive function to calculate total sales
let rec calculateTotalSalesRecursive orders total =
    match orders with
    | [] -> total
    | head :: tail -> calculateTotalSalesRecursive tail (total + head.orderTotalPrice)


(*..............................................................................*)


// Function to calculate total sales
let calculateTotalSales () =
    let orders = loadOrders "Orders.json"
    let totalSales = calculateTotalSalesRecursive orders 0.00M
    printfn "Total Sales: %M" totalSales

(*.....................................................................................*)

// Recursive function to calculate inventory value
let rec calculateInventoryValueRecursive (products:Product array) totalValue =
    match products with
    | [||] -> totalValue
    | _ ->
        let product = products.[0]
        let productValue = product.productPrice * decimal product.quantityInStock
        calculateInventoryValueRecursive (Array.tail products) (totalValue + productValue)


(*..........................................................................*)

// Function to calculate inventory value
let calculateInventoryValue () =
    let products = loadProductstoArr "Products.json"
    let totalValue = calculateInventoryValueRecursive products 0.00M
    printfn "Total Inventory Value: %M" totalValue

(*.............................................................................*)

// Main report function
let makeReport () =
    checkLowStockItems ()
    calculateTotalSales ()
    calculateInventoryValue ()
