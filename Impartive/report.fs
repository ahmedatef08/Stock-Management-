module StockManagement.report


open StockManagement.product

open StockManagement.order



let checkLoweStockItems()=
    let products = loadProducts "products.json"  

    for product in products do
       if product.quantityInStock < product.thresholdQuantity then          
           printfn "this poduct has lower stockItem Product: %s , Quantity: %d" product.productName  product.quantityInStock



let calculateTotalSales()=
   
    let orders = loadOrders "Orders.json" // Load existing orders

    let mutable totalSales = 0.00M

    for order in orders do

       totalSales <- totalSales + order.orderTotalPrice

    
    printfn "Total Sales: %M" totalSales



let calculateInventoryValue () =
    let products = loadProducts "products.json"  
    let mutable totalValue = 0.00M

    for product in products do
        totalValue <- totalValue + (product.productPrice * decimal product.quantityInStock)
    printfn "Total Inventory Value: %M" totalValue
       

   

let makereport()=
   checkLoweStockItems()
   calculateTotalSales()
   calculateInventoryValue()


