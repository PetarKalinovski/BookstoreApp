document.addEventListener('DOMContentLoaded', function () {
    const searchInput = document.querySelector('.search-input');
    let timeout = null;

    if (searchInput) {
        searchInput.addEventListener('input', function (e) {
            clearTimeout(timeout);

            timeout = setTimeout(() => {
                e.target.closest('form').submit();
            }, 500); 
        });
    }
});


function incrementQuantity() {
    const input = document.getElementById('quantity');
    const currentValue = parseInt(input.value);
    if (currentValue < 99) {
        input.value = currentValue + 1;
    }
}

function decrementQuantity() {
    const input = document.getElementById('quantity');
    const currentValue = parseInt(input.value);
    if (currentValue > 1) {
        input.value = currentValue - 1;
    }
}