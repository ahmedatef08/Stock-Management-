open StockManagement.product
open StockManagement.order
open StockManagement.incomingshipment
open StockManagement.report
open System

[<EntryPoint>]
let main _ =
    // Define a new product
    let newProduct = {
        productId = 3
        productName = "nokia"
        productPrice = 300.0m
        quantityInStock = 50
        thresholdQuantity = 15
    }


    let newShipment = {
        shipmentId = 2
        shipmentDate = DateTime.UtcNow
        quantityReceived = 10
        productId = 1
    }



    let newOrder ={ 
        orderId = 2
        orderDate = DateTime.Now
        productId = 1
        userId = 5001
        quantityOrdered = 5
        orderTotalPrice=0.0m
    }

   

    (*.......................Product..........................*)
    
    //addProduct "Products.json" newProduct 
    //updateProduct "Products.json" newProduct
    //let products = getProducts "Products.json"
    //printfn "%A" roducts
    //deleteProduct "Products.json" 3


    (*......................................................*)



    (*.......................IncomingShipment.......................*)


  
    //addIncomingShipment "IncomingShipment.json" newShipment


    (*.............................................................*)




    (*............................Orders.................................*)


    //makeOrder "Orders.json" newOrder


    (*.................................................*)


    (*.......................report............................*)

    //checkLoweStockItems()
    //calculateTotalSales()
    //calculateInventoryValue()
    makeReport()

     



    0

