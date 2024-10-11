// 微軟範例下載 https://github.com/dotnet/AspNetCore.Docs/tree/main/aspnetcore/tutorials/first-web-api/samples
// 官方說明文件 https://learn.microsoft.com/zh-tw/aspnet/core/tutorials/first-web-api?view=aspnetcore-7.0&tabs=visual-studio
// 搭配 /Models目錄。請搭配 wwwroot目錄下的 index.html 來使用。     

// <snippet_SiteJs>
const uri = 'api/todoitems';   //******** 請修改成自己的 api網址 ***
let todos = [];

// 參考文章 https://www.oxxostudio.tw/articles/201908/js-fetch.html
//fetch('網址')
//    .then(function (response) {
//        // 處理 response
//    }).catch(function (err) {
//        // 錯誤處理
//    });
// Fetch API的完整屬性說明，請看 https://developer.mozilla.org/en-US/docs/Web/API/Request

function getItems() {
// <snippet_GetItems>
  fetch(uri)
    .then(response => response.json())
    .then(data => _displayItems(data))
    .catch(error => console.error('Unable to get items.無法取得任何 items[待辦項目]', error));
// </snippet_GetItems>
}

// <snippet_AddItem>
function addItem() {
  const addNameTextbox = document.getElementById('add-name');

  const item = {
    isComplete: false,
    name: addNameTextbox.value.trim()
  };

  fetch(uri, {
    method: 'POST',
    headers: {
      'Accept': 'application/json',
      'Content-Type': 'application/json'
    },
    body: JSON.stringify(item)
  })
    .then(response => response.json())
    .then(() => {
      getItems();   //重新整理畫面，取得最新的待辦事項，並呈現在畫面上。
      addNameTextbox.value = '';
    })
    .catch(error => console.error('Unable to add item.無法新增[待辦項目]', error));
}
// </snippet_AddItem>

function deleteItem(id) {
  // <snippet_DeleteItem>
  fetch(`${uri}/${id}`, {
    method: 'DELETE'
  })
  .then(() => getItems())    //重新整理畫面，取得最新的待辦事項，並呈現在畫面上。
      .catch(error => console.error('Unable to delete item.無法刪除[待辦項目]', error));
  // </snippet_DeleteItem>
}

function displayEditForm(id) {
  const item = todos.find(item => item.id === id);
  
  document.getElementById('edit-name').value = item.name;
  document.getElementById('edit-id').value = item.id;
  document.getElementById('edit-isComplete').checked = item.isComplete;
  document.getElementById('editForm').style.display = 'block';
}

function updateItem() {
  const itemId = document.getElementById('edit-id').value;
  const item = {
    id: parseInt(itemId, 10),
    isComplete: document.getElementById('edit-isComplete').checked,
    name: document.getElementById('edit-name').value.trim()
  };

  // <snippet_UpdateItem>
  fetch(`${uri}/${itemId}`, {
    method: 'PUT',
    headers: {
      'Accept': 'application/json',
      'Content-Type': 'application/json'
    },
    body: JSON.stringify(item)
  })
  .then(() => getItems())    //重新整理畫面，取得最新的待辦事項，並呈現在畫面上。
      .catch(error => console.error('Unable to update item.無法更新[待辦項目]', error));
  // </snippet_UpdateItem>

  closeInput();

  return false;
}

function closeInput() {
  document.getElementById('editForm').style.display = 'none';
}

function _displayCount(itemCount) {
  const name = (itemCount === 1) ? 'to-do' : 'to-dos';

  document.getElementById('counter').innerText = `${itemCount} ${name}`;
}

function _displayItems(data) {
  const tBody = document.getElementById('todos');
  tBody.innerHTML = '';

  _displayCount(data.length);

  const button = document.createElement('button');

  data.forEach(item => {
    let isCompleteCheckbox = document.createElement('input');
    isCompleteCheckbox.type = 'checkbox';
    isCompleteCheckbox.disabled = true;
    isCompleteCheckbox.checked = item.isComplete;

    let editButton = button.cloneNode(false);
    editButton.innerText = 'Edit編輯';
    editButton.setAttribute('onclick', `displayEditForm(${item.id})`);

    let deleteButton = button.cloneNode(false);
    deleteButton.innerText = 'Delete刪除';
    deleteButton.setAttribute('onclick', `deleteItem(${item.id})`);

    let tr = tBody.insertRow();
    
    let td1 = tr.insertCell(0);
    td1.appendChild(isCompleteCheckbox);

    let td2 = tr.insertCell(1);
    let textNode = document.createTextNode(item.name);
    td2.appendChild(textNode);

    let td3 = tr.insertCell(2);
    td3.appendChild(editButton);

    let td4 = tr.insertCell(3);
    td4.appendChild(deleteButton);
  });

  todos = data;
}
// </snippet_SiteJs>
