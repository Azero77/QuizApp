// wwwroot/js/ExamView.js
function loadMathJax(src) {
    return new Promise((resolve, reject) => {
        const script = document.createElement('script');
        script.src = src;
        script.id = 'MathJax-script';
        script.async = true;
        script.onload = resolve;
        script.onerror = reject;
        document.head.appendChild(script);
    });
}

function renderMathInElement() {
    if (typeof MathJax !== 'undefined') {
        MathJax.typesetPromise();
    }
}

// Expose functions globally
window.loadMathJax = loadMathJax;
window.renderMathInElement = renderMathInElement;