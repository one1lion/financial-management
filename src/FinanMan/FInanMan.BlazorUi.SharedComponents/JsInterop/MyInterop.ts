export function showAlert(msg: string) {
    alert(msg);
}

export function capturePointerEvents(elem: HTMLElement) {
    elem.setPointerCapture(1);
}

export function releasePointerEvents(elem: HTMLElement) {
    elem.releasePointerCapture(1);
}