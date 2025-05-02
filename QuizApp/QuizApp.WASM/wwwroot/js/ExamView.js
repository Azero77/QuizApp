
// wwwroot/js/ExamView.js
function loadMathJax(src) {
    return new Promise((resolve, reject) => {
        if (document.getElementById('MathJax-script')) {
            console.log('MathJax script already loaded.');
            resolve();
            return;
        }

        const script = document.createElement('script');
        script.src = src;
        script.id = 'MathJax-script';
        script.async = true;
        script.onload = () => {
            console.log('MathJax script loaded successfully.');
            resolve();
        };
        script.onerror = (error) => {
            console.error('Failed to load MathJax script:', error);
            reject(error);
        };
        document.head.appendChild(script);
    });
}

function renderMathInElement() {
    if (typeof MathJax !== 'undefined') {
        // Add a small delay to ensure the DOM is fully updated
        setTimeout(() => {
            MathJax.typesetPromise()
                .then(() => console.log('MathJax rendering complete.'))
                .catch((error) => console.error('MathJax rendering failed:', error));
        }, 100); // Adjust delay if needed
    } else {
        console.error('MathJax is not defined.');
    }
}

// Expose functions globally
window.loadMathJax = loadMathJax;
window.renderMathInElement = renderMathInElement;
