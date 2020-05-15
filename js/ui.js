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
    document.title = title;
}

window.EnsureItemVisible = (item) => {
    var elm = document.getElementById("CK" + item).parentElement;
    elm.scrollIntoView({ block: 'center', behavior: 'smooth' });
}

window.FocusElement = (elm) => {
    elm.focus();
}

window.EnableTooltip = () => {
    try {
        $('tooltip').tooltip();
        $('li[data-toggle=\"tooltip\"]').tooltip();
        $('td[data-toggle=\"tooltip\"]').tooltip();
    } catch { }
}

window.DestroyTooltip = () => {
    try {
        $('div[class*="tooltip fade show"]').remove();
    }catch { }
}

window.GetBaseDirectory = () => {
    var Elm = document.getElementsByTagName("base")[0];
    return Elm.getAttribute("href");
}

function ShowDropDown(e) {
    e.classList.toggle("show");
}

function OpenFile(e) {
    e.click();
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
