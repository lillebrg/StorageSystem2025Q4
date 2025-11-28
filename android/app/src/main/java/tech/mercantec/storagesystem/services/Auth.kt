package tech.mercantec.storagesystem.services

import android.content.Context
import kotlinx.serialization.Serializable
import androidx.core.content.edit

@Serializable
data class BorrowedItem(val base_item_id: Int, val specific_item_id: Int, val base_item_name: String, val base_item_picture: String, val specific_item_description: String)

@Serializable
data class CurrentUser(val name: String, val email: String, val role: String, val borrowed_items: Array<BorrowedItem>, val change_password_on_next_login: Boolean)

class Auth(ctx: Context) {
    val api = Api(ctx)
    val prefs = ctx.getSharedPreferences("current_user", Context.MODE_PRIVATE)

    fun login(email: String, password: String) {
        @Serializable
        data class LoginRequest(val email: String, val password: String)

        @Serializable
        data class LoginResponse(val name: String, val email: String, val role: String, val access_token: String, val refresh_token: String, val change_password_on_next_login: Boolean)

        val request = LoginRequest(email, password)

        val response = api.requestJson<LoginRequest, LoginResponse>("POST", "/user/login", request)

        prefs.edit {
            putString("name", response.name)
            putString("email", response.email)
            putString("access_token", response.access_token)
            putBoolean("password_change_required", response.change_password_on_next_login)
        }
    }

    fun changePassword(currentPassword: String, newPassword: String) {
        @Serializable
        data class ChangePasswordRequest(val current_password: String, val new_password: String)

        val request = ChangePasswordRequest(currentPassword, newPassword)

        api.requestJson<ChangePasswordRequest, Boolean>("POST", "/user/change-password", request)
    }

    fun getCurrentUser(): CurrentUser {
        return api.requestJson<Unit, CurrentUser>("GET", "/user", null)
    }

    fun logout() {
        prefs.edit {
            remove("name")
            remove("email")
            remove("access_token")
            remove("password_change_required")
        }
    }

    fun isLoggedIn(): Boolean {
        return prefs.contains("access_token")
    }

    fun passwordChangeRequired(): Boolean {
        return prefs.getBoolean("password_change_required", false)
    }
}
