const canvas = document.getElementById('pistolCanvas');
const ctx = canvas.getContext('2d');

let clicks = 0;
let startTime = 0;
let isTesting = false;
let testDuration = 5000; // 5 seconds test
let finalCPS = 0;
let particles = [];
let targetRadius = 50;

// Draw Initial State
function draw() {
    ctx.fillStyle = '#111';
    ctx.fillRect(0, 0, canvas.width, canvas.height);

    // Draw Particles
    for (let i = particles.length - 1; i >= 0; i--) {
        let p = particles[i];
        p.x += p.vx;
        p.y += p.vy;
        p.life -= 0.05;

        ctx.fillStyle = `rgba(255, 93, 0, ${Math.max(0, p.life)})`;
        ctx.beginPath();
        ctx.arc(p.x, p.y, p.size, 0, Math.PI * 2);
        ctx.fill();

        if (p.life <= 0) particles.splice(i, 1);
    }

    if (!isTesting && startTime === 0 && finalCPS === 0) {
        ctx.fillStyle = '#FF5D00';
        ctx.font = '24px Jura, sans-serif';
        ctx.textAlign = 'center';
        ctx.fillText('CLICK THE TARGET AS FAST AS YOU CAN', canvas.width / 2, canvas.height / 2 - 20);
    }
    else if (isTesting) {
        let timeLeft = Math.max(0, (testDuration - (Date.now() - startTime)) / 1000);

        ctx.fillStyle = 'rgba(255, 93, 0, 0.1)';
        ctx.beginPath();
        ctx.arc(canvas.width / 2, canvas.height / 2, targetRadius + (Math.random() * 10), 0, Math.PI * 2);
        ctx.fill();

        // Target
        ctx.fillStyle = '#FF5D00';
        ctx.beginPath();
        ctx.arc(canvas.width / 2, canvas.height / 2, targetRadius, 0, Math.PI * 2);
        ctx.fill();
        ctx.lineWidth = 2;
        ctx.strokeStyle = '#FFF';
        ctx.stroke();

        ctx.fillStyle = 'white';
        ctx.font = '20px Roboto';
        ctx.textAlign = 'center';
        ctx.fillText(`${timeLeft.toFixed(1)}s`, canvas.width / 2, canvas.height / 2 - 70);
        ctx.font = '30px Jura';
        ctx.fillText(`CLICKS: ${clicks}`, canvas.width / 2, canvas.height / 2 + 90);
    }
    else if (!isTesting && finalCPS > 0) {
        ctx.fillStyle = '#FF5D00';
        ctx.font = '30px Jura, sans-serif';
        ctx.textAlign = 'center';
        ctx.fillText(`YOUR SPEED: ${finalCPS} CPS`, canvas.width / 2, canvas.height / 2 - 30);

        ctx.fillStyle = 'white';
        ctx.font = '16px Roboto';
        let msg = finalCPS > 10 ? "Fast for a human, but still slower than the macro." : "You're missing sub-ticks. Use the macro.";
        ctx.fillText(msg, canvas.width / 2, canvas.height / 2 + 10);

        // Compare to macro (62.5 CPS due to 16ms interval)
        ctx.fillStyle = '#ccc';
        ctx.fillText(`CS1337 MACRO SPEED: 62.5 CPS ZERO-JAM`, canvas.width / 2, canvas.height / 2 + 40);

        ctx.font = '14px Roboto';
        ctx.fillText(`Click to retry`, canvas.width / 2, canvas.height / 2 + 80);
    }

    // UI HUD
    ctx.textAlign = 'left';
    ctx.fillStyle = 'white';
    ctx.font = '12px Jura, sans-serif';
    ctx.fillText(`Macro Theoretical Max: 62.5 Clicks/sec (16ms per click)`, 10, 20);

    requestAnimationFrame(draw);
}

// Logic
canvas.addEventListener('mousedown', (e) => {
    // Math to determine if clicked inside target (center)
    let rect = canvas.getBoundingClientRect();
    let x = e.clientX - rect.left;
    let y = e.clientY - rect.top;

    if (!isTesting && startTime === 0) {
        // Start Test
        isTesting = true;
        startTime = Date.now();
        clicks = 1;
        finalCPS = 0;
        spawnParticles(canvas.width / 2, canvas.height / 2, 20);

        setTimeout(() => {
            isTesting = false;
            finalCPS = (clicks / (testDuration / 1000)).toFixed(1);
            startTime = 0;
            particles = [];
        }, testDuration);
    }
    else if (isTesting) {
        let dx = x - canvas.width / 2;
        let dy = y - canvas.height / 2;
        if (Math.sqrt(dx * dx + dy * dy) <= targetRadius + 10) {
            clicks++;
            targetRadius = 45; // shrink effect
            spawnParticles(x, y, 5);
            setTimeout(() => { targetRadius = 50; }, 50);
        }
    }
    else if (!isTesting && finalCPS > 0) {
        // Reset
        finalCPS = 0;
        draw();
    }
});

function spawnParticles(x, y, count) {
    for (let i = 0; i < count; i++) {
        particles.push({
            x: x, y: y,
            vx: (Math.random() - 0.5) * 10,
            vy: (Math.random() - 0.5) * 10,
            size: Math.random() * 3 + 1,
            life: 1
        });
    }
}

draw();
