function ShowDropDown(e) {
  e.classList.toggle("show");
}

function OpenFile(e) {
    e.click();
}

window.onclick = function(e) {
    if (!e.target.matches('.dropbtn')) {
        var DropDowns = document.getElementsByClassName('dropdown-content');
        for (var i = 0; i < DropDowns.length; i++) {
            var Dropdown = DropDowns[i];
            if (Dropdown.classList.contains('show')) {
                Dropdown.classList.remove('show');
            }
        }
  }
}