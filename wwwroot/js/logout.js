document.getElementById('logoutButton').onclick = function () {
    Swal.fire({
        title: "確定要登出嗎？",
        text: "您將退出當前賬戶",
        icon: "warning",
        showCancelButton: true,
        confirmButtonColor: "rgb(192 146 222)",
        cancelButtonColor: "#4F4F4F",
        confirmButtonText: "登出",
        cancelButtonText: "取消",
    }).then((result) => {
        if (result.isConfirmed) {
            document.getElementById('logoutForm').submit();
        }
    });
};