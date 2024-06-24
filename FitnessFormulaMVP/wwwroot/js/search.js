// search.js

function searchWorkouts() {
    const searchInput = document.getElementById('searchInput').value;
    if (!searchInput.trim()) {
        alert('Please enter a workout name to search.');
        return;
    }

    fetch(`https://localhost:7076/api/workouts/search?name=${encodeURIComponent(searchInput)}`)
        .then(response => {
            if (!response.ok) {
                throw new Error('Error fetching workouts: ' + response.statusText);
            }
            return response.json();
        })
        .then(data => {
            displayWorkouts(data);
        })
        .catch(error => {
            console.error('Error fetching workouts:', error);
            alert('Failed to fetch workouts. Please try again later.');
        });
}

function displayWorkouts(workouts) {
    const workoutsList = document.getElementById('workoutsList');
    workoutsList.innerHTML = ''; // Clear previous results

    if (workouts.length === 0) {
        workoutsList.innerHTML = '<p>No workouts found.</p>';
        return;
    }

    const workoutsHTML = workouts.map(workout => {
        return `
            <div class="workout">
                <h3>${workout.name}</h3>
                <p><strong>Category:</strong> ${workout.category}</p>
                <p><strong>Difficulty:</strong> ${workout.difficulty}</p>
                <p><strong>Duration:</strong> ${workout.duration} minutes</p>
            </div>
        `;
    }).join('');

    workoutsList.innerHTML = workoutsHTML;
}
