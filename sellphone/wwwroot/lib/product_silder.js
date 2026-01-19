window.initSlickSlider = function () {
    $('.responsive').slick({
        dots: false,
        infinite: false,
        speed: 300,
        slidesToShow: 5,
        slidesToScroll: 1,
        arrows: false,
        responsive: [
            {
                breakpoint: 1080,
                settings: {
                    slidesToShow: 4,
                    slidesToScroll: 1,
                    infinite: true,
                    dots: true
                }
            },
            {
                breakpoint: 900,
                settings: {
                    slidesToShow: 3,
                    slidesToScroll: 1
                }
            },
            {
                breakpoint: 600,
                settings: {
                    slidesToShow: 2,
                    slidesToScroll: 1
                }
            },
            {
                breakpoint: 480,
                settings: {
                    slidesToShow: 1,
                    slidesToScroll: 1
                }
            }
        ]
    });
    // Hàm này nhận vào 'action' là "prev" hoặc "next"
    window.controlSlider = function (action) {
        if (action === 'prev') {
            $('.responsive').slick('slickPrev'); // Lệnh lùi của Slick
        }
        else if (action === 'next') {
            $('.responsive').slick('slickNext'); // Lệnh tiến của Slick
        }
    };
};
