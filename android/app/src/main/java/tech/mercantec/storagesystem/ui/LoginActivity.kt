package tech.mercantec.storagesystem.ui

import android.content.Intent
import android.os.Bundle
import android.widget.Button
import android.widget.EditText
import android.widget.Toast
import androidx.activity.enableEdgeToEdge
import androidx.appcompat.app.AppCompatActivity
import tech.mercantec.storagesystem.R
import tech.mercantec.storagesystem.helpers.login
import tech.mercantec.storagesystem.helpers.makeApiRequest

class LoginActivity : AppCompatActivity() {
    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        enableEdgeToEdge()
        setContentView(R.layout.activity_login)

        findViewById<Button>(R.id.login_button).setOnClickListener {
            val email = findViewById<EditText>(R.id.email_input).text.toString()
            val password = findViewById<EditText>(R.id.password_input).text.toString()

            makeApiRequest(this, { login(email, password) }) { result ->
                Toast.makeText(this, result.email, Toast.LENGTH_LONG).show()

                val intent = Intent(this, MainActivity::class.java)
                intent.flags = Intent.FLAG_ACTIVITY_NEW_TASK or Intent.FLAG_ACTIVITY_CLEAR_TASK // Prevent going back to login screen
                startActivity(intent)
            }
        }
    }
}