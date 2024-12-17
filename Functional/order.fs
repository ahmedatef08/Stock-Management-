
module StockManagement.order

open System
open System.IO
open Newtonsoft.Json
open StockManagement.product

type Order = {
    orderId: int
    orderDate: System.DateTime
    productId: int
    userId: int
    quantityOrdered: int
    orderTotalPrice: decimal
}

(*...............................................*)

// Function to load orders
let loadOrders filePath =
    if File.Exists filePath then
        let json = File.ReadAllText filePath
        JsonConvert.DeserializeObject<Order[]>(json) |> Array.toList
    else
        []

(*..............................................*)


let loadProductstoArr filePath =
    if File.Exists filePath then
        let json = File.ReadAllText filePath
        JsonConvert.DeserializeObject<Product[]>(json) // Deserialize JSON to a Product array
    else
        [||] // Return an empty array if the file doesn't exist





(*................................................*)

// Function to save orders
let saveOrders filePath orders =
    let json = JsonConvert.SerializeObject(orders, Formatting.Indented)
    File.WriteAllText(filePath, json)

(*................................................................*)

// Helper function to check if an order exists
let rec orderExists orderId orders =
    match orders with
    | [] -> false
    | head :: tail when head.orderId = orderId -> true
    | _ :: tail -> orderExists orderId tail


(*......................................................................*)

// Helper function to find a product by ID
let rec findProductById productId (products: Product array) =
    match products with
    | [||] -> None
    | _ when products.[0].productId = productId -> Some products.[0]
    | _ -> findProductById productId (Array.tail products)


(*...................................................................*)

// Helper function to update the product stock
let rec updateProductStock (products: Product array) (updatedProduct: Product) =
    match products with
    | [||] -> [||]
    | _ when products.[0].productId = updatedProduct.productId -> Array.append [| updatedProduct |] (Array.tail products)
    | _ -> Array.append [| products.[0] |] (updateProductStock (Array.tail products) updatedProduct)


(*........................................................................*)

// Function to create a new order
let makeOrder filePath newOrder =
    let orders = loadOrders filePath
    let products: Product array = loadProductstoArr "Products.json" // Ensure products are loaded as Product array

    if orderExists newOrder.orderId orders then
        printfn "Error: Order with ID %d already exists." newOrder.orderId
    else
        match findProductById newOrder.productId products with
        | None -> printfn "Error: Product with ID %d not found." newOrder.productId
        | Some product ->
            if product.quantityInStock < newOrder.quantityOrdered then
                printfn "Error: Insufficient stock for product ID %d." newOrder.productId
            else
                let updatedProduct = {
                    productId = product.productId
                    productName = product.productName
                    productPrice = product.productPrice
                    quantityInStock = product.quantityInStock - newOrder.quantityOrdered
                    thresholdQuantity = product.thresholdQuantity
                }

                let updatedProducts = updateProductStock products updatedProduct
                saveProducts "products.json" updatedProducts

                let newOrderWithTotalPrice = {
                    newOrder with orderTotalPrice = decimal newOrder.quantityOrdered * product.productPrice
                }

                let updatedOrders = newOrderWithTotalPrice :: orders
                saveOrders filePath updatedOrders

                printfn "Order with ID %d added successfully." newOrderWithTotalPrice.orderId


(*..................................................................................*)