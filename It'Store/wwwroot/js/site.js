function changecolor() {
    var scroll = $(window).scrollTop();
    if (scroll > 40) {
        $("header").css("background", "#121212");
        $(".main-header").css("box-shadow", "0 0 12px rgba(0,0,0,.2)");
    }

    else {
        $(".main-header").css("background", "rgba(255,0,255,0)");
        $(".main-header").css("box-shadow", "0 0 0 rgba(0,0,0,0)");
    }
}
function function2() {
    //еще одна функция выполняющаяся при скролле
};
function function3() {
    //еще одна функция на загрузку страницы
};
$(document).ready(function () {

    //это тест для начальной прокрутки стриницы
    //window.scrollBy в вашем варианте не нужен
    window.scrollBy(0, 100);

    changecolor();
    function3();
    $(window).scroll(function () {
        changecolor();
        function2();
    });
})

