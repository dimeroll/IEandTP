const uri = 'api/Customers';
let customers = [];

function getCustomers() {
    fetch(uri)
        .then(response => response.json())
        .then(data => _displayCustomers(data))
        .catch(error => console.error('Unable to get customers.', error));
}

function addCustomer() {
    const addNameTextbox = document.getElementById('add-name');
    const addDateTextbox = document.getElementById('add-date');
    var parts = addDateTextbox.value.trim().split('-');

    const customer = {
        name: addNameTextbox.value.trim(),
        DateOfBirth: new Date(parts[0], parts[1], parts[2], 0, 0, 0)
    };

    fetch(uri, {
        method: 'POST',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(customer)
    })
        .then(response => response.json())
        .then(() => {
            getCustomers();
            addNameTextbox.value = '';
            addDateTextbox.value = '';
        })
        .catch(error => console.error('Unable to add customer.', error));
}

function deleteCustomer(id) {
    fetch(`${uri}/${id}`, {
        method: 'DELETE'
    })
        .then(() => getCustomers())
        .catch(error => console.error('Unable to delete customer.', error));
}

function displayEditForm(id) {
    const customer = customers.find(customer => customer.id === id);

    document.getElementById('edit-id').value = customer.id;
    document.getElementById('edit-name').value = customer.name;
    document.getElementById('edit-date').value = '';
    document.getElementById('editForm').style.display = 'block';
}

function updateCustomer() {
    const customerId = document.getElementById('edit-id').value;
    var parts = document.getElementById('edit-date').value.trim().split('-');
    const customer = {
        id: parseInt(customerId, 10),
        name: document.getElementById('edit-name').value.trim(),
        DateOfBirth: new Date(parts[0], parts[1], parts[2], 0, 0, 0)
    };

    fetch(`${uri}/${customerId}`, {
        method: 'PUT',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(customer)
    })
        .then(() => getCustomers())
        .catch(error => console.error('Unable to update customer.', error));

    closeInput();

    return false;
}

function closeInput() {
    document.getElementById('editForm').style.display = 'none';
}


function _displayCustomers(data) {
    const tBody = document.getElementById('customers');
    tBody.innerHTML = '';


    const button = document.createElement('button');

    data.forEach(customer => {
        let editButton = button.cloneNode(false);
        editButton.innerText = 'Edit';
        editButton.setAttribute('onclick', `displayEditForm(${customer.id})`);

        let deleteButton = button.cloneNode(false);
        deleteButton.innerText = 'Delete';
        deleteButton.setAttribute('onclick', `deleteCustomer(${customer.id})`);

        let tr = tBody.insertRow();


        let td1 = tr.insertCell(0);
        let textNode = document.createTextNode(customer.name);
        td1.appendChild(textNode);

        let td2 = tr.insertCell(1);
        /*var dd = '0' + customer.DateOfBirth.getDate();
        var mm = '0' + customer.DateOfBirth.getMonth();
        var yy = '0' + customer.DateOfBirth.getFullYear();*/
        let textNodeDate = document.createTextNode('-');
        td2.appendChild(textNodeDate);

        let td3 = tr.insertCell(2);
        td3.appendChild(editButton);

        let td4 = tr.insertCell(3);
        td4.appendChild(deleteButton);
    });

    customers = data;
}
