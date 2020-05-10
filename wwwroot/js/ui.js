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

window.SetTitle = (title) => {
    console.log('Setting title', title);
    document.title = title;
}

window.EnsureItemVisible = (item) => {
    var elm = document.getElementById("CK" + item).parentElement;
    elm.scrollIntoView({ block: 'center', behavior: 'smooth' });
}

window.FocusElement = (elm) => {
    elm.focus();
}

async function Initialize() {
    if (typeof (ReadyFlag) == "undefined")
        return;

    if (Interval !== null) {
        clearInterval(Interval);
        Interval = null;
    }

    await DotNet.invokeMethodAsync('Sermone', 'Initialize');
    setTimeout(Initialize, 10);
}

var Interval = setInterval(Initialize, 100);