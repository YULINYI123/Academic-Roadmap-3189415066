# Academic Roadmap Generator 🎓

Academic Roadmap Generator is a minimalist, Notion-inspired web application designed to help students and lifelong learners bridge the gap between "wanting to learn" and "knowing how to learn." It generates structured, week-by-week learning paths for any subject and allows users to export these goals directly to their calendars.

## 🌐 Live Demo
**📍 点击这里体验在线版本：** https://YULINYI123.github.io/Academic-Roadmap-3189415066/

> **使用说明**：打开链接后，选择左侧的学位类型（如 Computer Science），系统自动生成 4 周学习计划。无需账号，所有数据存储在浏览器本地。

## 🎯 Motivation
Modern learners are often overwhelmed by the sheer volume of information available. The biggest hurdle is often the first step: organizing a logical sequence of study. This tool provides a clear starting point and actionable milestones to ensure consistent progress.

## ✨ Features
- **Instant Roadmap Generation**: Get a 4-week structured plan for any topic
- **Progress Tracking**: Check off tasks as you go; progress saved automatically
- **Smart Templates**: Specialized paths for Computer Science, Data Science, Business Analytics, etc.
- **Task Management**: Click to edit tasks, use ✕ button to delete unwanted tasks
- **GPA Calculator**: Automatically calculate cumulative GPA based on course grades
- **ICS Calendar Export**: One-click export to Google Calendar, Apple Calendar, or Outlook
- **Glassmorphism Design**: Beautiful, responsive UI with smooth animations
- **Local Storage**: All data persists in your browser; no account needed

## 📸 Screenshots
![Academic Roadmap Generator main interface](screenshot.png)

The screenshot shows the roadmap generator interface with academic templates, generated weekly tasks, progress tracking, and the GPA calculator panel.

## 🚀 How to Use

### Online (Recommended)
Simply visit: https://YULINYI123.github.io/Academic-Roadmap-3189415066/

### Local Development
1. Clone the repository:
   ```bash
   git clone https://github.com/YULINYI123/Academic-Roadmap-3189415066.git
   cd Academic-Roadmap-3189415066
   ```
2. Open `index.html` in any modern web browser
3. No build step required (Pure HTML/JavaScript/CSS)

### Verification Guide
See [GitHub_Actions_验证指南.md](GitHub_Actions_验证指南.md) for:
- How to verify the automatic deployment via GitHub Actions
- Where to check the deployment status (green checkmarks ✅)
- Troubleshooting if something goes wrong

## 📋 Project Structure
```
├── index.html              # Main application UI
├── script.js              # Core functionality (150+ lines)
├── style.css              # Glassmorphism design (550+ lines)
├── README.md              # This file
├── .github/workflows/deploy.yml  # Automatic GitHub Pages deployment
└── screenshot.png         # Project screenshot for README
```

## 🔧 Technology Stack
- **Frontend**: HTML5, CSS3, Vanilla JavaScript (ES6+)
- **Design Pattern**: Glassmorphism with smooth animations
- **Storage**: Browser LocalStorage API
- **Deployment**: GitHub Pages with GitHub Actions CI/CD

## 📊 Git Commit History
This project demonstrates consistent, incremental development:
```
✅ docs: Add project README with features and motivation
✅ feat: Build HTML structure and main UI components
✅ style: Implement glassmorphism design and responsive layout
✅ feat: Add roadmap generation, progress tracking, and calendar export functionality
✅ feat: Add task editing and deletion functionality
```

Each commit represents a meaningful development phase, not last-minute updates.

## 🎓 Learning Outcomes
By building this project, the developer demonstrated:
- Frontend UI/UX design with CSS animations
- JavaScript event handling and DOM manipulation
- Data structure and algorithm design (roadmap generation)
- Local storage and state management
- Git version control best practices

## 📝 License
MIT

---

**Questions or Issues?** See [GitHub_PAGES_部署完整验证指南.md](../GitHub_Pages_部署完整验证指南.md) for deployment verification steps.
