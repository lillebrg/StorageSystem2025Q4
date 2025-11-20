package tech.mercantec.storagesystem.services

import android.app.Activity
import android.app.ProgressDialog
import android.content.Context
import android.util.Log
import android.widget.Toast
import kotlinx.serialization.*
import kotlinx.serialization.json.Json
import java.io.IOException
import java.net.*
import tech.mercantec.storagesystem.BuildConfig
import kotlin.concurrent.thread

class ApiRequestException(message: String, cause: Throwable?) : Exception(message, cause)

class HttpResponse(val body: String, val code: Int)

class Api(val ctx: Context) {
    fun request(method: String, path: String, data: String?): HttpResponse {
        val url = URL(BuildConfig.API_BASE_URL + path)

        val accessToken = ctx.getSharedPreferences("current_user", Context.MODE_PRIVATE).getString("access_token", null)

        try {
            with(url.openConnection() as HttpURLConnection) {
                requestMethod = method

                if (accessToken != null)
                    setRequestProperty("Authorization", "Bearer $accessToken")

                if (data != null) {
                    setRequestProperty("Content-Type", "application/json")

                    outputStream.write(data.toByteArray())
                    outputStream.flush()
                }

                if (responseCode >= 400)
                    return HttpResponse(errorStream.readBytes().decodeToString(), responseCode)

                return HttpResponse(inputStream.readBytes().decodeToString(), responseCode)
            }
        } catch (e: IOException) {
            Log.e("StorageSystem", e.toString())

            throw ApiRequestException("Failed to connect to server: " + e.message, e)
        }
    }

    @Serializable
    class HttpErrorResponse(val message: String)

    val json = Json { explicitNulls = false } // Treat missing values as null when decoding

    inline fun <reified Req, reified Res> requestJson(
        method: String,
        path: String,
        data: Req?
    ): Res {
        val requestJson =
            if (data != null)
                Json.encodeToString(serializer<Req>(), data)
            else null

        val response = request(method, path, requestJson)

        if (response.code >= 400) {
            try {
                val error = Json.decodeFromString<HttpErrorResponse>(response.body)

                throw ApiRequestException(error.message, null)
            } catch (e: SerializationException) {
                if (e.message != null)
                    Log.e("StorageSystem", e.message!!)
                Log.e("StorageSystem", response.body)

                throw ApiRequestException(
                    "Request failed with HTTP status code ${response.code}",
                    e
                )
            }
        }

        if (response.body.isBlank()) {
            // Return Unit or an empty default value depending on Res
            return when (Res::class) {
                Unit::class -> Unit as Res
                else -> throw ApiRequestException("Expected JSON but got empty response", null)
            }
        }

        try {
            return json.decodeFromString<Res>(response.body)
        } catch (e: SerializationException) {
            if (e.message != null)
                Log.e("StorageSystem", e.message!!)
            Log.e("StorageSystem", response.body)

            throw ApiRequestException("Failed to parse response: ${response.body}", e)
        }
    }

    companion object Helpers {
        fun <T> makeRequest(ctx: Activity, request: () -> T, showLoading: Boolean = true, callback: (T) -> Unit) {
            var progressDialog: ProgressDialog? = null

            if (showLoading) {
                progressDialog = ProgressDialog(ctx).apply {
                    setMessage("Loading...")
                    show()
                }
            }

            thread {
                val result: T
                try {
                    result = request()
                } catch (e: ApiRequestException) {
                    ctx.runOnUiThread {
                        Toast.makeText(ctx, e.message, Toast.LENGTH_LONG).show()
                    }

                    return@thread
                } finally {
                    ctx.runOnUiThread {
                        progressDialog?.hide()
                    }
                }

                ctx.runOnUiThread {
                    callback(result)
                }
            }
        }
    }
}
