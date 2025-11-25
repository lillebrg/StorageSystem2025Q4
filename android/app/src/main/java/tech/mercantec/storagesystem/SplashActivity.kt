package tech.mercantec.storagesystem

import android.content.Intent
import android.os.Bundle
import android.widget.Toast
import androidx.appcompat.app.AppCompatActivity
import tech.mercantec.storagesystem.services.*
import tech.mercantec.storagesystem.ui.*

class SplashActivity : AppCompatActivity() {
    override fun onCreate(savedInstanceState: Bundle?) {
        val auth = Auth(this)

        if (auth.isLoggedIn()) {
            val activity =
                if (auth.passwordChangeRequired())
                    ChangePasswordActivity::class.java
                else
                    MainActivity::class.java

            val intent = Intent(this, activity)
            intent.flags = Intent.FLAG_ACTIVITY_NEW_TASK or Intent.FLAG_ACTIVITY_CLEAR_TASK
            startActivity(intent)

            Api.makeRequest(this, { auth.getCurrentUser() }, showLoading = false) { user ->
                Toast.makeText(this, "Welcome back, ${user.name}", Toast.LENGTH_SHORT).show()
            }
        } else {
            startActivity(Intent(this, LoginActivity::class.java))
        }

        super.onCreate(savedInstanceState)
    }
}
