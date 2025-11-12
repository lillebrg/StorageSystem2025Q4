package tech.mercantec.storagesystem

import android.content.Intent
import android.os.Bundle
import androidx.appcompat.app.AppCompatActivity
import tech.mercantec.storagesystem.ui.LoginActivity

class SplashActivity : AppCompatActivity() {
    override fun onCreate(savedInstanceState: Bundle?) {
        // TODO check if logged in

        startActivity(Intent(this, LoginActivity::class.java))

        super.onCreate(savedInstanceState)
    }
}
