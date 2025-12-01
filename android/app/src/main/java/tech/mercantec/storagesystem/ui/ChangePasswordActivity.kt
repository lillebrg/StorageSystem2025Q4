package tech.mercantec.storagesystem.ui

import android.content.Intent
import android.os.Bundle
import android.view.View
import android.widget.*
import androidx.activity.enableEdgeToEdge
import androidx.appcompat.app.AppCompatActivity
import tech.mercantec.storagesystem.R
import tech.mercantec.storagesystem.services.*
import kotlin.concurrent.thread

class ChangePasswordActivity : AppCompatActivity() {
    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        enableEdgeToEdge()
        setContentView(R.layout.activity_change_password)

        if (intent.extras?.containsKey("currentPassword") == true) {
            findViewById<TextView>(R.id.current_password_label).visibility = View.GONE
            findViewById<EditText>(R.id.current_password_input).visibility = View.GONE
        }

        val auth = Auth(this)

        findViewById<Button>(R.id.reset_password_button).setOnClickListener {
            val currentPassword = intent.extras?.getString("currentPassword") ?: findViewById<EditText>(R.id.current_password_input).text.toString()
            val newPassword = findViewById<EditText>(R.id.new_password_input).text.toString()
            val confirmPassword = findViewById<EditText>(R.id.confirm_password_input).text.toString()

            if (newPassword != confirmPassword) {
                Toast.makeText(this, "Passwords do not match", Toast.LENGTH_LONG).show()
                return@setOnClickListener
            }

            Api.makeRequest(this, { auth.changePassword(currentPassword, newPassword) }) {
                val intent = Intent(this, MainActivity::class.java)
                intent.flags = Intent.FLAG_ACTIVITY_NEW_TASK or Intent.FLAG_ACTIVITY_CLEAR_TASK
                startActivity(intent)
            }
        }

        findViewById<Button>(R.id.logout_button).setOnClickListener {
            thread { auth.logout() }

            val intent = Intent(this, LoginActivity::class.java)
            intent.flags = Intent.FLAG_ACTIVITY_NEW_TASK or Intent.FLAG_ACTIVITY_CLEAR_TASK
            startActivity(intent)
        }
    }
}
