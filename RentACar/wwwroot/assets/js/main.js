


let btncar = document.querySelectorAll(".btncar");
let img = document.querySelectorAll(".cat-img");


getCar();
btncar.forEach(x => {
    
    x.addEventListener("click", function () {
        let id = $(this).attr("data-id");
        for (var i = 0; i < img.length; i++) {
            let dtid = img[i].getAttribute("data-target");
            if (id == dtid) {
                img[i].classList.add("active");
            }
            else {
                img[i].classList.remove("active");
            }
        }
    })
})


function getCar() {
    btncar.forEach(x => {
        let id = $(x).attr("data-id");
        for (var i = 0; i < img.length; i++) {
            let dtid = img[i].getAttribute("data-target");
            if (id == dtid) {
                img[i].classList.add("active");
            }
            else {
                img[i].classList.remove("active");
            }
        }
    })
}


