pipeline {
    agent any

    stages {
        stage('Build') {
            when {
                branch 'main'
            }
            steps {
                echo 'Building..'
            }
        }
        stage('Deploy') {
            when {
                branch 'main'
                tag "release-*"
            }
            steps {
                echo 'Deploying....'
            }
        }
    }
}
