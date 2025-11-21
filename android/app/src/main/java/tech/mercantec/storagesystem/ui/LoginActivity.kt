package tech.mercantec.storagesystem.ui

import android.content.Intent
import android.os.Bundle
import android.widget.*
import androidx.activity.enableEdgeToEdge
import androidx.appcompat.app.AppCompatActivity
import tech.mercantec.storagesystem.R
import tech.mercantec.storagesystem.services.*

class LoginActivity : AppCompatActivity() {
    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        enableEdgeToEdge()
        setContentView(R.layout.activity_login)

        val auth = Auth(this)

        findViewById<Button>(R.id.login_button).setOnClickListener {
            val email = findViewById<EditText>(R.id.email_input).text.toString()
            val password = findViewById<EditText>(R.id.password_input).text.toString()

            Api.makeRequest(this, { auth.login(email, password) }) {
                val activity =
                    if (auth.passwordChangeRequired())
                        ChangePasswordActivity::class.java
                    else
                        MainActivity::class.java

                val intent = Intent(this, activity)
                intent.flags = Intent.FLAG_ACTIVITY_NEW_TASK or Intent.FLAG_ACTIVITY_CLEAR_TASK // Prevent going back to login screen
                if (auth.passwordChangeRequired()) intent.putExtra("currentPassword", password)
                startActivity(intent)

                Api.makeRequest(this, { auth.getCurrentUser() }, showLoading = false) { user ->
                    Toast.makeText(this, "Welcome, ${user.name}", Toast.LENGTH_SHORT).show()
                }
            }
        }
    }
}