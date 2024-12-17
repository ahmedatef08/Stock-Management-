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

let loadOrders filePath =
    if File.Exists filePath then
        let json = File.ReadAllText filePath
        JsonConvert.DeserializeObject<Order[]>(json) |> Array.toList
    else
        []

(*..............................................*)


let saveOrders filePath orders =
    let json = JsonConvert.SerializeObject(orders, Formatting.Indented)
    File.WriteAllText(filePath, json)


(*................................................................*)

let makeOrder filePath newOrder =
    let orders = loadOrders filePath // Load existing orders
    let products = loadProducts "products.json" // Ensure products exist to validate productId

    let mutable orderExists = false
    let mutable productFound = false
    let mutable validQuantity = false
    let mutable calculatedOrderTotalPrice = 0.0m // To store the calculated total price

    // Check if the order ID already exists
    for order in orders do
        if order.orderId = newOrder.orderId then
            orderExists <- true

    if orderExists then
        printfn "Error: Order with ID %d already exists." newOrder.orderId
    else
        // Validate product ID and quantity
        for product in products do
            if product.productId = newOrder.productId then
                productFound <- true
                if product.quantityInStock >= newOrder.quantityOrdered then
                    validQuantity <- true
                    // Calculate the order total price
                    calculatedOrderTotalPrice <- decimal newOrder.quantityOrdered * product.productPrice

                    // Update product stock
                    let updatedProduct = {
                        productId = product.productId
                        productName = product.productName
                        productPrice = product.productPrice
                        quantityInStock = product.quantityInStock - newOrder.quantityOrdered
                        thresholdQuantity = product.thresholdQuantity
                    }
                    updateProduct "products.json" updatedProduct

        if not productFound then
            printfn "Error: Product with ID %d not found." newOrder.productId
        elif not validQuantity then
            printfn "Error: Insufficient stock for product ID %d." newOrder.productId
        else
            // Create a new order with the calculated total price
            let newOrderWithNewPrice = { newOrder with orderTotalPrice = calculatedOrderTotalPrice }

            // Add the order and save
            let updatedOrders = orders @ [newOrderWithNewPrice]
            saveOrders filePath updatedOrders
            printfn "Order with ID %d added successfully." newOrderWithNewPrice.orderId



(*.......................................................................*)
