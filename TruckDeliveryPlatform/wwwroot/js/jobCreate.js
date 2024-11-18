function updateCost() {
    const pickupId = $('#PickupLocationId').val();
    const dropoffId = $('#DropoffLocationId').val();
    
    console.log('Pickup ID:', pickupId);
    console.log('Dropoff ID:', dropoffId);
    
    if (pickupId && dropoffId) {
        const url = `/api/locations/distance?fromId=${pickupId}&toId=${dropoffId}`;
        console.log('Making request to:', url);
        
        $.get(url)
            .done(function(data) {
                console.log('Response:', data);
                $('#estimatedDistance').text(`${data.distance.toFixed(2)} km`);
                $('#estimatedCost').text(`${data.cost.toFixed(2)} EGP`);
            })
            .fail(function(jqXHR, textStatus, errorThrown) {
                console.error('Error:', textStatus, errorThrown);
            });
    }
}

$(document).ready(function() {
    console.log('JobCreate.js loaded');
    $('#PickupLocationId, #DropoffLocationId').change(function() {
        console.log('Dropdown changed');
        updateCost();
    });
}); 