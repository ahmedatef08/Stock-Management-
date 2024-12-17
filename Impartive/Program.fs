
open StockManagement.product
open StockManagement.incomingshippment
open StockManagement.order
open StockManagement.report
open System

[<EntryPoint>]
let main _ =
    // Define a new product
    let newProduct = {
        productId = 2
        productName = "samsung ultra22"
        productPrice = 300.0m
        quantityInStock = 50
        thresholdQuantity = 15
    }

    let newShipment = {
        shipmentId = 2
        shipmentDate = DateTime.UtcNow
        quantityReceived = 100
        productId = 1
    }

    let newOrder = {
        orderId = 2
        orderDate = DateTime.Now
        productId = 1
        userId = 5001
        quantityOrdered = 5
        orderTotalPrice=0.0m
    }

    (*.......................Product..........................*)
    
    // addProduct "products.json" newProduct 
    // updateProduct "products.json" newProduct
    // let products = getProducts "products.json"
    // printfn "%A" products
    // deleteProduct "products.json" 2


    (*......................................................*)



    (*.......................IncomingShipment.......................*)


  
    //addIncomingShipment "IncomingShipments.json" newShipment


    (*.............................................................*)




    (*............................Orders.................................*)


    //makeOrder "Orders.json" newOrder


    (*.................................................*)


    (*.......................report............................*)

    //checkLoweStockItems()
    //calculateTotalSales()
    //calculateInventoryValue()
    makereport()

     



    0
