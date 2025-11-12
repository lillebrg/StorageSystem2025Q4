package tech.mercantec.storagesystem.helpers

import kotlinx.serialization.Serializable

@Serializable
data class LoginRequest(val email: String, val password: String)

@Serializable
data class LoginResponse(val name: String, val email: String, val role: String, val access_token: String)

fun login(email: String, password: String): LoginResponse {
    val request = LoginRequest(email, password)

    return requestJson<LoginRequest, LoginResponse>("POST", "/user/login", request)
}
