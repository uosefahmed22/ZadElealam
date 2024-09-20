# Zad ElEalem

**Zad ElEalem** is an educational platform designed for learning Islamic studies, offering a variety of subjects like Fiqh, Sharia, and more. The platform provides a structured approach with video playlists, progress tracking, and certificates of appreciation for students upon completing courses.

---

## 🚀 Features

1. **📚 Sections and Subjects**  
   Organized into various religious subjects, such as Fiqh and Sharia, each subject is systematically structured to facilitate learning.

2. **🎥 Video Playlists**  
   Subjects are linked with YouTube playlists, allowing students to follow along and track their progress as they watch.

3. **📊 Student Progress Tracking**  
   The platform saves students' progress in each subject, displaying completion percentages to keep learners motivated.

4. **🎓 Certificates of Appreciation**  
   Upon course completion, students receive certificates of appreciation that can be downloaded as PDFs.

5. **💬 Interaction and Q&A**  
   Allows interaction between students and teachers through comments and questions, enabling dynamic learning and feedback.

6. **⭐ Course Rating System**  
   Students can rate courses and leave feedback, helping to improve content quality and guide future learners.

7. **📈 Personal Dashboard**  
   Each student has a personalized page displaying their enrolled courses, achievements, and earned certificates.

---

## 🧑‍💼 User Roles

1. **Student**  
   The primary user who joins the platform to learn and engage with the courses.

2. **Sheikh/Teacher**  
   The educator who provides and teaches the content.

3. **Admin**  
   The individual responsible for managing the entire platform, including content, users, and overall functionality.

---

## 🛠️ Getting Started

To get started with Zad ElEalem, follow these instructions:

1. **Clone the repository**:
   ```bash
   git clone https://github.com/yourusername/ZadElEalem.git
   ```

2. **Set up the environment**:
   Open the project in Visual Studio and ensure the correct environment setup based on the project requirements.

3. **Run the project**:
   ```bash
   dotnet run
   ```

---

## 📑 API Endpoints

### Auth

- `POST /api/Auth/register`  
  Register a new user.

- `POST /api/Auth/login`  
  Login for registered users.

- `POST /api/Auth/refresh-token`  
  Refresh JWT token.

- `POST /api/Auth/revoke-token`  
  Revoke JWT token.

- `POST /api/Auth/forgot-password`  
  Send password reset email.

- `POST /api/Auth/verify-otp`  
  Verify OTP for account recovery.

- `POST /api/Auth/reset-password`  
  Reset the user's password.

- `POST /api/Auth/resend-confirmation-email`  
  Resend the email confirmation link.

- `GET /api/Auth/confirm-email`  
  Confirm the user's email.

- `POST /api/Auth/change-password`  
  Change user password.

### Category

- `GET /api/Category/getAllCategories`  
  Retrieve all categories.

- `GET /api/Category/getCategoryById`  
  Get a category by ID.

- `POST /api/Category/addCategory`  
  Add a new category.

- `PUT /api/Category/updateCategory`  
  Update an existing category.

- `DELETE /api/Category/deleteCategory`  
  Delete a category.

### Certificate

- `GET /api/Certificate/download`  
  Download the certificate of appreciation.

### Exam

- `POST /api/Exam/create-exam`  
  Create a new exam.

- `GET /api/Exam/get-exam-by-playlist`  
  Get an exam by YouTube playlist.

- `POST /api/Exam/submit-exam`  
  Submit a completed exam.

- `GET /api/Exam/get-all-student-exams`  
  Retrieve all student exams.

- `DELETE /api/Exam/delete-exam`  
  Delete an exam.

- `GET /api/Exam/get-exam-by-id`  
  Get exam details by ID.

### Feedback and Favorites

- `POST /api/FeedbackAndFavorites/addFeedback`  
  Add feedback for a playlist.

- `GET /api/FeedbackAndFavorites/getAllFeedbackByPlaylist`  
  Retrieve all feedback for a specific playlist.

- `DELETE /api/FeedbackAndFavorites/deleteFeedback`  
  Delete feedback.

- `PUT /api/FeedbackAndFavorites/updateFeedback`  
  Update feedback.

- `POST /api/FeedbackAndFavorites/addFavorites`  
  Add an item to favorites.

- `GET /api/FeedbackAndFavorites/getAllFavorites`  
  Retrieve all favorite items.

- `DELETE /api/FeedbackAndFavorites/deleteFavorites`  
  Remove an item from favorites.

### Notification

- `GET /api/Notification/GetNotificationsForUserAsync`  
  Get all notifications for a user.

- `POST /api/Notification/markNotificationAsRead`  
  Mark notification as read.

- `POST /api/Notification/markNotificationAsUnread`  
  Mark notification as unread.

- `DELETE /api/Notification/deleteNotification`  
  Delete a notification.

- `POST /api/Notification/createNotification`  
  Create a new notification.

### User Role

- `GET /api/UserRole/GetRoles`  
  Retrieve all roles.

- `POST /api/UserRole/CreateRole`  
  Create a new role.

- `DELETE /api/UserRole/DeleteRole`  
  Delete a role.

- `POST /api/UserRole/AddUserToRole`  
  Assign a role to a user.

- `DELETE /api/UserRole/RemoveUserFromRole`  
  Remove a role from a user.

- `GET /api/UserRole/GetUsers`  
  Get all users.

- `GET /api/UserRole/GetRoleByUser`  
  Get the role assigned to a user.

- `PATCH /api/UserRole/AddProfileImage`  
  Add a profile image for a user.

- `DELETE /api/UserRole/DeleteUser`  
  Delete a user.

### YouTube Playlist

- `POST /api/YouTubePlaylist/addPlaylist`  
  Add a new YouTube playlist.

- `GET /api/YouTubePlaylist/getPlaylistsByCategory`  
  Retrieve playlists by category.

- `GET /api/YouTubePlaylist/getVideosByPlaylist`  
  Retrieve videos from a playlist.

- `DELETE /api/YouTubePlaylist/deletePlaylist`  
  Delete a playlist.

- `PUT /api/YouTubePlaylist/updateVideoProgress`  
  Update the progress of a video.
