package tech.mercantec.storagesystem.ui

import android.content.Intent
import android.os.Bundle
import android.widget.Button
import android.widget.EditText
import android.widget.Toast
import androidx.activity.enableEdgeToEdge
import androidx.appcompat.app.AppCompatActivity
import tech.mercantec.storagesystem.R
import tech.mercantec.storagesystem.services.Api
import tech.mercantec.storagesystem.services.Auth

class LoginActivity : AppCompatActivity() {
    val auth = Auth(this)

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        enableEdgeToEdge()
        setContentView(R.layout.activity_login)

        findViewById<Button>(R.id.login_button).setOnClickListener {
            val email = findViewById<EditText>(R.id.email_input).text.toString()
            val password = findViewById<EditText>(R.id.password_input).text.toString()

            Api.makeRequest(this, { auth.login(email, password) }) {
                val intent = Intent(this, MainActivity::class.java)
                intent.flags = Intent.FLAG_ACTIVITY_NEW_TASK or Intent.FLAG_ACTIVITY_CLEAR_TASK // Prevent going back to login screen
                startActivity(intent)

                val user = auth.getCurrentUser()
                Toast.makeText(this, "Welcome, ${user.name}", Toast.LENGTH_SHORT).show()
            }
        }
    }
}