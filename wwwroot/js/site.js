// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

function ConfirmOrderRemoval(event) {
   
    result = confirm("Vill du verkligen ta bort denna ordern. Det går inte att ångra borttagning av en order");
    if (result == true) {
        return;
    }
    else {
        event.preventDefault();         
    }
}

function RemoveProductFromShop(event) {
   
    result = confirm("Vill du ta bort denna podukten fån webshopen. Det går inte att ta bort bort en produkt helt och hållet eftersom det förstör gammal försäljningstatistik");
    if (result == true) {
        return;
    }
    else {
        event.preventDefault();         
    }
}
