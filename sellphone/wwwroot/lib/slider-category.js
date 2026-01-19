// 1. Hàm khởi tạo (Chỉ lo việc tạo slider)
window.initCategorySlider = function () {
    var $slider = $('.cate-slider');

    // Kiểm tra xem đã chạy chưa, nếu rồi thì hủy để chạy lại (tránh lỗi)
    if ($slider.hasClass('slick-initialized')) {
        $slider.slick('unslick');
    }

    $slider.slick({
        dots: false,
        infinite: false,
        speed: 300,
        slidesToShow: 7,
        slidesToScroll: 1,
        arrows: false, // Tắt mũi tên mặc định
        responsive: [
            {
                breakpoint: 1100,
                settings: {
                    slidesToShow: 5,
                    slidesToScroll: 1,
                    infinite: true,
                    dots: true
                }
            },
            {
                breakpoint: 900,
                settings: {
                    slidesToShow: 4,
                    slidesToScroll: 1
                }
            },
            {
                breakpoint: 600,
                settings: {
                    slidesToShow: 3,
                    slidesToScroll: 1
                }
            },
            {
                breakpoint: 480,
                settings: {
                    slidesToShow: 2,
                    slidesToScroll: 1
                }
            }
        ]
    });
};

// 2. Hàm điều khiển nút bấm 
window.CastecontrolSlider = function (action) {   
    var $slider = $('.cate-slider');

    if (action === 'prev') {
        $slider.slick('slickPrev');
    }
    else if (action === 'next') {
        $slider.slick('slickNext');
    }
};