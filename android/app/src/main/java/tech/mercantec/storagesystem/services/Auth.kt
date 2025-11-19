package tech.mercantec.storagesystem.services

import android.content.Context
import kotlinx.serialization.Serializable
import androidx.core.content.edit

data class CurrentUser(val name: String, val email: String, val role: String)

class Auth(val ctx: Context) {
    val api = Api(ctx)

    fun login(email: String, password: String) {
        @Serializable
        data class LoginRequest(val email: String, val password: String)

        @Serializable
        data class LoginResponse(val name: String, val email: String, val role: String, val access_token: String)

        val request = LoginRequest(email, password)

        val response = api.requestJson<LoginRequest, LoginResponse>("POST", "/user/login", request)

        ctx.getSharedPreferences("current_user", Context.MODE_PRIVATE).edit {
            putString("name", response.name)
            putString("email", response.email)
            putString("access_token", response.access_token)
        }
    }

    fun getCurrentUser(): CurrentUser {
        with(ctx.getSharedPreferences("current_user", Context.MODE_PRIVATE)) {
            val name = getString("name", "Unknown")!!
            val email = getString("email", "Unknown")!!
            val role = getString("role", "Unknown")!!

            return CurrentUser(name, email, role)
        }
    }

    fun logout() {
        ctx.getSharedPreferences("current_user", Context.MODE_PRIVATE).edit {
            remove("name")
            remove("email")
            remove("access_token")
        }
    }

    fun isLoggedIn(): Boolean {
        return ctx.getSharedPreferences("current_user", Context.MODE_PRIVATE).contains("access_token")
    }
}