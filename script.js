document.addEventListener('DOMContentLoaded', () => {
    const subjectInput = document.getElementById('subject-input');
    const generateBtn = document.getElementById('generate-btn');
    const roadmapSection = document.getElementById('roadmap-section');
    const roadmapTitle = document.getElementById('roadmap-title');
    const roadmapContent = document.getElementById('roadmap-content');
    const exportBtn = document.getElementById('export-btn');

    let currentRoadmap = null;

    const roadmapTemplates = {
        'default': [
            { week: 1, title: 'Foundations', tasks: ['Introduction to core concepts', 'Setting up the environment', 'First small project'] },
            { week: 2, title: 'Deep Dive', tasks: ['Understanding advanced syntax', 'Core architecture patterns', 'Building a prototype'] },
            { week: 3, title: 'Optimization', tasks: ['Performance tuning', 'Best practices and security', 'Refactoring code'] },
            { week: 4, title: 'Final Project', tasks: ['Building a complete application', 'Deployment and testing', 'Documentation'] }
        ],
        'coding': [
            { week: 1, title: 'Language Basics', tasks: ['Syntax and variables', 'Control flow and functions', 'Basic data structures'] },
            { week: 2, title: 'Frameworks', tasks: ['Learning the ecosystem', 'Component architecture', 'State management'] },
            { week: 3, title: 'API & Data', tasks: ['Fetching data', 'Authentication', 'Database integration'] },
            { week: 4, title: 'Deployment', tasks: ['CI/CD setup', 'Cloud hosting', 'Monitoring'] }
        ],
        'mathematics': [
            { week: 1, title: 'Set Theory & Logic', tasks: ['Basic terminology', 'Truth tables', 'Universal/Existential quantifiers'] },
            { week: 2, title: 'Calculus I', tasks: ['Limits and continuity', 'Differentiation rules', 'Applications of derivatives'] },
            { week: 3, title: 'Calculus II', tasks: ['Integration techniques', 'Definite integrals', 'Area and volume'] },
            { week: 4, title: 'Linear Algebra', tasks: ['Matrices and vectors', 'Systems of equations', 'Eigenvalues and eigenvectors'] }
        ],
        'language': [
            { week: 1, title: 'Pillars of Language', tasks: ['Basic alphabet and pronunciation', 'Essential greetings', 'Sentence structure basics'] },
            { week: 2, title: 'Daily Communication', tasks: ['Workplace vocabulary', 'Shopping and eating out', 'Social interactions'] },
            { week: 3, title: 'Grammar Deep Dive', tasks: ['Verb conjugations', 'Tense variations', 'Complex sentence connectors'] },
            { week: 4, title: 'Immersion', tasks: ['Watching native media', 'Writing a short essay', 'Casual conversation practice'] }
        ],
        'art': [
            { week: 1, title: 'Sketching Basics', tasks: ['Shape and form', 'Light and shadow', 'Perspective drawing'] },
            { week: 2, title: 'Color Theory', tasks: ['Primary, secondary, tertiary colors', 'Color harmony', 'Mixing techniques'] },
            { week: 3, title: 'Composition', tasks: ['Rule of thirds', 'Leading lines', 'Focal points'] },
            { week: 4, title: 'Personal Style', tasks: ['Medium experimentation', 'Drafting a final piece', 'Portfolio presentation'] }
        ],
        'history': [
            { week: 1, title: 'Ancient Era', tasks: ['Mesopotamia and Egypt', 'Classical Greece', 'Roman Empire'] },
            { week: 2, title: 'Middle Ages', tasks: ['Feudalism', 'The Renaissance', 'Global exploration'] },
            { week: 3, title: 'Industrial Revolution', tasks: ['Technological leaps', 'Social changes', 'Rise of modern nations'] },
            { week: 4, title: 'Contemporary History', tasks: ['World Wars', 'Cold War era', 'Digital transformation/Modern era'] }
        ]
    };

    // Load progress from localStorage
    function loadProgress(subject) {
        const saved = localStorage.getItem(`roadmap_${subject}`);
        return saved ? JSON.parse(saved) : null;
    }

    // Save progress to localStorage
    function saveProgress(subject, progress) {
        localStorage.setItem(`roadmap_${subject}`, JSON.stringify(progress));
    }

    generateBtn.addEventListener('click', () => {
        const subject = subjectInput.value.trim();
        if (!subject) return;

        generateRoadmap(subject);
    });

    function generateRoadmap(subject) {
        roadmapTitle.innerText = `Roadmap for ${subject}`;
        roadmapSection.classList.remove('hidden');
        
        // Subject selection logic
        let templateKey = 'default';
        const s = subject.toLowerCase();
        if (s.includes('code') || s.includes('data') || s.includes('python') || s.includes('java')) templateKey = 'coding';
        else if (s.includes('math') || s.includes('calculus') || s.includes('algebra')) templateKey = 'mathematics';
        else if (s.includes('language') || s.includes('english') || s.includes('spanish') || s.includes('chinese')) templateKey = 'language';
        else if (s.includes('art') || s.includes('paint') || s.includes('draw')) templateKey = 'art';
        else if (s.includes('history') || s.includes('ancient') || s.includes('war')) templateKey = 'history';
        
        const template = roadmapTemplates[templateKey];
        
        // Try to load saved individual progress
        const savedProgress = loadProgress(subject);
        
        currentRoadmap = template.map((item, wIndex) => ({
            ...item,
            subject: subject,
            tasks: item.tasks.map((t, tIndex) => ({
                text: t,
                completed: savedProgress ? (savedProgress[wIndex]?.tasks[tIndex]?.completed || false) : false
            }))
        }));

        renderRoadmap(currentRoadmap);
        showDashboard();
    }

    function showDashboard() {
        document.getElementById('roadmap-dashboard').classList.remove('hidden');
        updateProgress();
    }

    function updateProgress() {
        if (!currentRoadmap) return;
        const allTasks = currentRoadmap.flatMap(w => w.tasks);
        const total = allTasks.length;
        const done = allTasks.filter(t => t.completed).length;
        const percent = total === 0 ? 0 : Math.round((done / total) * 100);
        
        document.getElementById('total-progress').style.width = `${percent}%`;
        document.getElementById('progress-text').innerText = `${percent}%`;
        document.getElementById('tasks-count').innerText = `${done} / ${total}`;
    }

    function renderRoadmap(roadmap) {
        roadmapContent.innerHTML = '';
        roadmap.forEach((item, wIndex) => {
            const card = document.createElement('div');
            card.className = 'week-card';
            
            const title = document.createElement('div');
            title.className = 'week-title';
            title.innerHTML = `<span>Week ${item.week}</span> ${item.title}`;
            
            const list = document.createElement('ul');
            list.className = 'task-list';
            item.tasks.forEach((task, tIndex) => {
                const li = document.createElement('li');
                li.className = `task-item ${task.completed ? 'completed' : ''}`;
                
                const checkbox = document.createElement('input');
                checkbox.type = 'checkbox';
                checkbox.checked = task.completed;
                checkbox.addEventListener('change', () => {
                    task.completed = checkbox.checked;
                    li.classList.toggle('completed', task.completed);
                    saveProgress(item.subject, currentRoadmap);
                    updateProgress();
                });

                const textSpan = document.createElement('span');
                textSpan.innerText = task.text;
                
                li.appendChild(checkbox);
                li.appendChild(textSpan);
                list.appendChild(li);
            });
            
            card.appendChild(title);
            card.appendChild(list);
            roadmapContent.appendChild(card);
        });

        // Scroll to roadmap
        roadmapSection.scrollIntoView({ behavior: 'smooth' });
    }

    exportBtn.addEventListener('click', () => {
        if (!currentRoadmap) return;
        downloadICS(currentRoadmap);
    });

    function downloadICS(roadmap) {
        const subject = roadmap[0].subject;
        let icsContent = [
            'BEGIN:VCALENDAR',
            'VERSION:2.0',
            'PRODID:-//Academic Roadmap Generator//EN'
        ];

        const now = new Date();
        
        roadmap.forEach((item, index) => {
            const startDate = new Date(now);
            startDate.setDate(now.getDate() + (index * 7));
            const endDate = new Date(startDate);
            endDate.setDate(startDate.getDate() + 1);

            const formatICSDate = (date) => {
                return date.toISOString().replace(/[-:]/g, '').split('.')[0] + 'Z';
            };

            icsContent.push('BEGIN:VEVENT');
            icsContent.push(`SUMMARY:${subject} - Week ${item.week}: ${item.title}`);
            icsContent.push(`DTSTART:${formatICSDate(startDate)}`);
            icsContent.push(`DTEND:${formatICSDate(endDate)}`);
            icsContent.push(`DESCRIPTION:${item.tasks.map(t => t.text).join('\\n')}`);
            icsContent.push('END:VEVENT');
        });

        icsContent.push('END:VCALENDAR');
        
        const blob = new Blob([icsContent.join('\r\n')], { type: 'text/calendar;charset=utf-8' });
        const link = document.createElement('a');
        link.href = window.URL.createObjectURL(blob);
        link.download = `${subject.replace(/\s+/g, '_')}_Roadmap.ics`;
        document.body.appendChild(link);
        link.click();
        document.body.removeChild(link);
    }
    // Tab Navigation Logic
    const tabBtns = document.querySelectorAll('.tab-btn');
    const tabContents = document.querySelectorAll('.tab-content');

    tabBtns.forEach(btn => {
        btn.addEventListener('click', () => {
            const target = btn.getAttribute('data-tab');
            
            tabBtns.forEach(b => b.classList.remove('active'));
            tabContents.forEach(c => c.classList.remove('active'));
            
            btn.classList.add('active');
            document.getElementById(`${target}-view`).classList.add('active');
        });
    });

    // Semester Planner Logic
    const courseNameInput = document.getElementById('course-name');
    const courseCreditsInput = document.getElementById('course-credits');
    const courseGradeInput = document.getElementById('course-grade');
    const addCourseBtn = document.getElementById('add-course-btn');
    const courseListBody = document.getElementById('course-list-body');
    const gpaScoreDisplay = document.getElementById('gpa-score');

    let courses = JSON.parse(localStorage.getItem('academic_courses')) || [];

    function saveCourses() {
        localStorage.setItem('academic_courses', JSON.stringify(courses));
        calculateGPA();
    }

    function calculateGPA() {
        if (courses.length === 0) {
            gpaScoreDisplay.innerText = '0.00';
            return;
        }

        let totalPoints = 0;
        let totalCredits = 0;

        courses.forEach(course => {
            totalPoints += parseFloat(course.grade) * parseInt(course.credits);
            totalCredits += parseInt(course.credits);
        });

        const gpa = totalPoints / totalCredits;
        gpaScoreDisplay.innerText = gpa.toFixed(2);
    }

    function renderCourses() {
        courseListBody.innerHTML = '';
        courses.forEach((course, index) => {
            const row = document.createElement('tr');
            row.innerHTML = `
                <td>${course.name}</td>
                <td>${course.credits}</td>
                <td>${course.grade}</td>
                <td><button class="delete-btn" data-index="${index}">Remove</button></td>
            `;
            courseListBody.appendChild(row);
        });
        
        // Add event listeners to delete buttons
        document.querySelectorAll('.delete-btn').forEach(btn => {
            btn.addEventListener('click', (e) => {
                const index = e.target.getAttribute('data-index');
                removeCourse(index);
            });
        });

        calculateGPA();
    }

    function removeCourse(index) {
        courses.splice(index, 1);
        saveCourses();
        renderCourses();
    }

    addCourseBtn.addEventListener('click', () => {
        const name = courseNameInput.value.trim();
        const credits = courseCreditsInput.value;
        const grade = courseGradeInput.value;

        if (!name || !credits) return;

        courses.push({ name, credits, grade });
        saveCourses();
        renderCourses();

        courseNameInput.value = '';
        courseCreditsInput.value = '';
    });

    // Demo Logic
    const loadRoadmapDemoBtn = document.getElementById('load-roadmap-demo');
    const loadPlannerDemoBtn = document.getElementById('load-planner-demo');

    loadRoadmapDemoBtn.addEventListener('click', () => {
        subjectInput.value = 'Web Development';
        generateRoadmap('Web Development');
    });

    loadPlannerDemoBtn.addEventListener('click', () => {
        courses = [
            { name: 'Advanced Algorithms', credits: 4, grade: '4.0' },
            { name: 'Machine Learning', credits: 3, grade: '3.7' },
            { name: 'Operating Systems', credits: 4, grade: '3.3' },
            { name: 'Database Systems', credits: 3, grade: '4.0' }
        ];
        saveCourses();
        renderCourses();
    });

    // Theme Toggle
    const themeToggle = document.getElementById('theme-toggle');
    const body = document.body;

    if (localStorage.getItem('theme') === 'dark') {
        body.classList.add('dark-mode');
    }

    themeToggle.addEventListener('click', () => {
        body.classList.toggle('dark-mode');
        const isDark = body.classList.contains('dark-mode');
        localStorage.setItem('theme', isDark ? 'dark' : 'light');
    });

    // Initial render
    renderCourses();
});



