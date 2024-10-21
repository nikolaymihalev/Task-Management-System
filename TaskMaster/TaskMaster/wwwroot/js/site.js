function changeSidebarActiveButton(index) {
    const sidebarLinkEls = document.querySelectorAll('#sidebarMenu a');

    for (el of sidebarLinkEls) {
        el.classList.remove('active');
    }

    sidebarLinkEls[index].classList.add('active');
}
